using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using NodaTime;
// ReSharper disable InconsistentNaming

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators.Internal;

internal static class InstantUtilities
{
    public static SqlExpression Plus(
        SqlExpression instant,
        SqlExpression duration,
        ISqlExpressionFactory factory,
        RelationalTypeMapping doubleTypeMapping)
    {
        instant = UnwrapStrftimeNow(instant, factory);
        var unixSeconds = TryUnwrapStrftime(instant, out var value)
            ? value
            : UnixEpoch(instant, factory, doubleTypeMapping);
        duration = factory.ChangeExpressionType(duration, doubleTypeMapping);
        instant = factory.Add(unixSeconds, duration, doubleTypeMapping);
        return Strftime(instant, factory);
    }

    public static SqlExpression Minus(
        SqlExpression instant,
        SqlExpression duration,
        ISqlExpressionFactory factory,
        RelationalTypeMapping doubleTypeMapping)
    {
        instant = UnwrapStrftimeNow(instant, factory);
        var unixSeconds = TryUnwrapStrftime(instant, out var value)
            ? value
            : UnixEpoch(instant, factory, doubleTypeMapping);
        duration = factory.ChangeExpressionType(duration, doubleTypeMapping);
        instant = factory.Subtract(unixSeconds, duration, doubleTypeMapping);
        return Strftime(instant, factory);
    }

    /// <summary>
    /// Calls <c>unixepoch(instant, 'subsec')</c>
    /// </summary>
    public static SqlExpression UnixEpoch(
        SqlExpression instant,
        ISqlExpressionFactory sqlExpressionFactory,
        RelationalTypeMapping doubleTypeMapping)
    {
        Debug.Assert(doubleTypeMapping.ClrType == typeof(double));

        return sqlExpressionFactory.Function(
            unixepoch,
            [instant, sqlExpressionFactory.Constant("subsec")],
            true,
            [true, false],
            typeof(double),
            doubleTypeMapping);
    }

    /// <summary>
    /// Calls <c>strftime(DateFormat, instant, 'unixepoch')</c>
    /// </summary>
    private static SqlExpression Strftime(SqlExpression value, ISqlExpressionFactory factory)
    {
        return factory.Function(
            strftime,
            [factory.Constant(Constants.DateTimeFormat), value, factory.Constant(unixepoch)],
            true,
            [false, true, false],
            typeof(Instant));
    }

    /// <summary>
    /// If the expression is a function call <c>strftime(..., value, 'unixepoch')</c>, unwraps it to just value.
    /// </summary>
    private static bool TryUnwrapStrftime(SqlExpression instant, [MaybeNullWhen(false)] out SqlExpression value)
    {
        if (instant is SqlFunctionExpression
            {
                Name: strftime,
                Arguments: [
                    SqlConstantExpression { Value: Constants.DateTimeFormat },
                    _,
                    SqlConstantExpression { Value: unixepoch }
                ]
            } function)
        {
            value = function.Arguments[1];
            return true;
        }
        value = default;
        return false;
    }


    /// <summary>
    /// Checks if the expression is a function call <c>strftime(..., 'now')</c>
    /// i.e. translation of <c>SystemClock.Instance.GetCurrentInstant()</c>,
    /// and returns 'now' if so.
    /// </summary>
    public static SqlExpression UnwrapStrftimeNow(SqlExpression expression, ISqlExpressionFactory factory)
    {
        const string now = "now";
        return expression is SqlFunctionExpression
            {
                Name: strftime,
                Arguments: [_, SqlConstantExpression { Value: now }]
            }
            ? factory.Constant(now)
            : expression;
    }

    private const string strftime = "strftime";
    private const string unixepoch = "unixepoch";
}
