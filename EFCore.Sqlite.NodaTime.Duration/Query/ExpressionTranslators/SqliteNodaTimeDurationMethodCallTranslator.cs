using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators;

internal class SqliteNodaTimeDurationMethodCallTranslator : IMethodCallTranslator
{
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    private readonly RelationalTypeMapping _doubleTypeMapping;
    private readonly RelationalTypeMapping _durationTypeMapping;

    public SqliteNodaTimeDurationMethodCallTranslator(ISqlExpressionFactory sqlExpressionFactory, ITypeMappingSource typeMappingSource)
    {
        _sqlExpressionFactory = sqlExpressionFactory;

        _doubleTypeMapping = typeMappingSource.GetRelationalTypeMapping(typeof(double));
        _durationTypeMapping = typeMappingSource.GetRelationalTypeMapping(typeof(NodaTime.Duration));
    }

    private static readonly MethodInfo _durationBetween =
        typeof(SqliteNodaTimeDurationDbFunctionsExtensions).GetRuntimeMethod(
            nameof(SqliteNodaTimeDurationDbFunctionsExtensions.DurationBetween),
            [typeof(DbFunctions), typeof(Instant), typeof(Instant)])!;

    public SqlExpression? Translate(
        SqlExpression? instance,
        MethodInfo method,
        IReadOnlyList<SqlExpression> arguments,
        IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (method == _durationBetween)
        {
            Debug.Assert(arguments.Count == 3);
            return TranslateDurationBetween(arguments[1], arguments[2]);
        }

        if (method.DeclaringType == typeof(NodaTime.Duration) && method.IsStatic)
        {
            return TranslateDuration(method, arguments);
        }

        return null;
    }

    /// <summary>
    /// Translates static methods on <see cref="Duration"/>
    /// </summary>
    private SqlExpression? TranslateDuration(
        MethodInfo method,
        IReadOnlyList<SqlExpression> arguments)
    {
        if (arguments.Count != 1)
        {
            return null; // All supported methods have exactly one argument
        }

        var argument = arguments[0];

        // Convert argument to double if it isn't already
        if (argument.TypeMapping?.DbType != DbType.Double)
        {
            argument = _sqlExpressionFactory.Convert(argument, typeof(double), _doubleTypeMapping);
        }

        return method.Name switch
        {
            nameof(NodaTime.Duration.FromSeconds) =>
                // Value is already in seconds, we just need to change the type
                _sqlExpressionFactory.ChangeExpressionType(argument, _durationTypeMapping),

            nameof(NodaTime.Duration.FromNanoseconds) =>
                _sqlExpressionFactory.Divide(
                    argument,
                    _sqlExpressionFactory.Constant(DurationConstants.NanosecondsPerSecond, _doubleTypeMapping),
                    _durationTypeMapping),

            nameof(NodaTime.Duration.FromMilliseconds) =>
                _sqlExpressionFactory.Divide(
                    argument,
                    _sqlExpressionFactory.Constant(DurationConstants.MillisecondsPerSecond, _doubleTypeMapping),
                    _durationTypeMapping),

            nameof(NodaTime.Duration.FromMinutes) =>
                _sqlExpressionFactory.Multiply(
                    argument,
                    _sqlExpressionFactory.Constant(DurationConstants.SecondsPerMinute, _doubleTypeMapping),
                    _durationTypeMapping),

            nameof(NodaTime.Duration.FromHours) =>
                _sqlExpressionFactory.Multiply(
                    argument,
                    _sqlExpressionFactory.Constant(DurationConstants.SecondsPerHour, _doubleTypeMapping),
                    _durationTypeMapping),

            nameof(NodaTime.Duration.FromDays) =>
                _sqlExpressionFactory.Multiply(
                    argument,
                    _sqlExpressionFactory.Constant(DurationConstants.SecondsPerDay, _doubleTypeMapping),
                    _durationTypeMapping),

            _ => null
        };
    }

    private SqlExpression TranslateDurationBetween(SqlExpression start, SqlExpression end)
    {
        start = UnixEpoch(start);
        end = UnixEpoch(end);

        // Get seconds difference
        return _sqlExpressionFactory.Subtract(end, start, _durationTypeMapping);

        SqlExpression UnixEpoch(SqlExpression instant)
        {
            Debug.Assert(instant.Type == typeof(Instant));

            // Optimization: if instant is strftime(..., 'now'), we can replace with 'now'
            // Result is then 'unixepoch('now', 'subsec')' rather than 'unixepoch(strftime(..., 'now'), 'subsec')'
            if (IsStrftimeNow(instant))
            {
                instant = _sqlExpressionFactory.Constant("now");
            }

            return _sqlExpressionFactory.Function(
                "unixepoch",
                [instant, _sqlExpressionFactory.Constant("subsec")],
                true,
                [true, false],
                typeof(double),
                _doubleTypeMapping);
        }
    }

    /// <summary>
    /// Checks if the expression is a function call <c>strftime(..., 'now')</c>
    /// i.e. translation of <c>SystemClock.Instance.GetCurrentInstant()</c>
    /// </summary>
    private static bool IsStrftimeNow(SqlExpression expression)
        => expression is SqlFunctionExpression { Name: "strftime", Arguments.Count: 2 } fn
           && fn.Arguments[1] is SqlConstantExpression { Value: "now" };
}
