using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators.Internal;

internal class SqliteNodaTimeDurationMethodCallTranslator : IMethodCallTranslator
{
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    private readonly RelationalTypeMapping _doubleTypeMapping;
    private readonly RelationalTypeMapping _durationTypeMapping;

    public SqliteNodaTimeDurationMethodCallTranslator(ISqlExpressionFactory sqlExpressionFactory, ITypeMappingSource typeMappingSource)
    {
        _sqlExpressionFactory = sqlExpressionFactory;

        _doubleTypeMapping = typeMappingSource.GetRelationalTypeMapping(typeof(double));
        _durationTypeMapping = typeMappingSource.GetRelationalTypeMapping(typeof(Duration));
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

        if (method.DeclaringType == typeof(Duration) && method.IsStatic)
        {
            return TranslateDuration(method, arguments);
        }

        if (instance == null)
        {
            return null;
        }

        if (method.DeclaringType == typeof(Instant))
        {
            return TranslateInstant(instance, method, arguments);
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
            nameof(Duration.FromSeconds) =>
                // Value is already in seconds, we just need to change the type
                _sqlExpressionFactory.ChangeExpressionType(argument, _durationTypeMapping),

            nameof(Duration.FromNanoseconds) =>
                _sqlExpressionFactory.Divide(
                    argument,
                    _sqlExpressionFactory.Constant(DurationConstants.NanosecondsPerSecond, _doubleTypeMapping),
                    _durationTypeMapping),

            nameof(Duration.FromMilliseconds) =>
                _sqlExpressionFactory.Divide(
                    argument,
                    _sqlExpressionFactory.Constant(DurationConstants.MillisecondsPerSecond, _doubleTypeMapping),
                    _durationTypeMapping),

            nameof(Duration.FromMinutes) =>
                _sqlExpressionFactory.Multiply(
                    argument,
                    _sqlExpressionFactory.Constant(DurationConstants.SecondsPerMinute, _doubleTypeMapping),
                    _durationTypeMapping),

            nameof(Duration.FromHours) =>
                _sqlExpressionFactory.Multiply(
                    argument,
                    _sqlExpressionFactory.Constant(DurationConstants.SecondsPerHour, _doubleTypeMapping),
                    _durationTypeMapping),

            nameof(Duration.FromDays) =>
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

            instant = InstantUtilities.UnwrapStrftimeNow(instant, _sqlExpressionFactory);
            return InstantUtilities.UnixEpoch(instant, _sqlExpressionFactory, _doubleTypeMapping);
        }
    }

    private SqlExpression? TranslateInstant(
        SqlExpression instance,
        MethodInfo method,
        IReadOnlyList<SqlExpression> arguments)
    {
        Debug.Assert(method.DeclaringType == typeof(Instant));
        return method.Name switch
        {
            nameof(Instant.Plus) => InstantUtilities.Plus(instance, arguments[0], _sqlExpressionFactory, _doubleTypeMapping),
            nameof(Instant.Minus) => InstantUtilities.Minus(instance, arguments[0], _sqlExpressionFactory, _doubleTypeMapping),
            _ => null
        };
    }
}
