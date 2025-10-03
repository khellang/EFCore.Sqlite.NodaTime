using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Sqlite.Extensions;
using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators.Internal;

internal class SqliteNodaTimeMemberTranslator(ISqlExpressionFactory sqlExpressionFactory) : IMemberTranslator
{
    private static readonly MemberInfo _systemClockInstance =
        typeof(SystemClock).GetRuntimeProperty(nameof(SystemClock.Instance))!;

    public SqlExpression? Translate(
        SqlExpression? instance,
        MemberInfo member,
        Type returnType,
        IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (member == _systemClockInstance)
        {
            return sqlExpressionFactory.Constant(SystemClock.Instance);
        }

        if (instance is null)
        {
            return null;
        }

        var declaringType = member.DeclaringType;
        if (declaringType == typeof(LocalDateTime) ||
            declaringType == typeof(LocalDate) ||
            declaringType == typeof(LocalTime))
        {
            return TranslateDateTime(instance, member, returnType);
        }

        return null;
    }

    private SqlExpression? TranslateDateTime(SqlExpression instance, MemberInfo member, Type returnType)
    {
        return member.Name switch
        {
            nameof(LocalDate.Year) => sqlExpressionFactory.Strftime(returnType, "%Y", instance),
            nameof(LocalDate.Month) => sqlExpressionFactory.Strftime(returnType, "%m", instance),
            nameof(LocalDate.Day) => sqlExpressionFactory.Strftime(returnType, "%d", instance),

            nameof(LocalDate.DayOfYear) => sqlExpressionFactory.Strftime(returnType, "%j", instance),
            nameof(LocalDate.DayOfWeek) => TranslateDayOfWeek(returnType, instance),

            nameof(LocalTime.Hour) => sqlExpressionFactory.Strftime(returnType, "%H", instance),
            nameof(LocalTime.Minute) => sqlExpressionFactory.Strftime(returnType, "%M", instance),
            nameof(LocalTime.Second) => sqlExpressionFactory.Strftime(returnType, "%S", instance),
            nameof(LocalTime.Millisecond) => null, // Ugh :(

            nameof(LocalDateTime.Date) => sqlExpressionFactory.Date(returnType, instance),
            // Unfortunately we can't use the time convenience function if we want to support fractional seconds :(
            nameof(LocalDateTime.TimeOfDay) => sqlExpressionFactory.Strftime(returnType, Constants.TimeFormat, instance),

            _ => null
        };
    }

    /// <summary>
    /// Unlike DateTime.DayOfWeek, NodaTime's IsoDayOfWeek enum doesn't exactly correspond to sqlite's
    /// values returned by strftime('%w', ...): in NodaTime Sunday is 7 and not 0, which is None.
    /// So we generate a CASE WHEN expression to translate sqlite's 0 to 7.
    /// </summary>
    private SqlExpression TranslateDayOfWeek(Type returnType, SqlExpression instance)
    {
        var value = sqlExpressionFactory.Strftime(returnType, "%w", instance);
        var test = sqlExpressionFactory.Equal(value, sqlExpressionFactory.Constant(0));
        var cases = new[] { new CaseWhenClause(test, sqlExpressionFactory.Constant(7)) };
        return sqlExpressionFactory.Case(cases, value);
    }
}
