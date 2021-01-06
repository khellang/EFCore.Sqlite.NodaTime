using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Sqlite.Extensions;
using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators.Internal
{
    public class SqliteNodaTimeMethodCallTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo _getCurrentInstant =
            typeof(SystemClock).GetRuntimeMethod(nameof(SystemClock.GetCurrentInstant), Type.EmptyTypes)!;

        private readonly SqlExpression[] _getCurrentInstantArgs;

        public SqliteNodaTimeMethodCallTranslator(ISqlExpressionFactory sqlExpressionFactory)
        {
            SqlExpressionFactory = sqlExpressionFactory;
            _getCurrentInstantArgs = new SqlExpression[]
            {
                SqlExpressionFactory.Constant(Constants.DateTimeFormat),
                SqlExpressionFactory.Constant("now"),
            };
        }

        private ISqlExpressionFactory SqlExpressionFactory { get; }

        public SqlExpression? Translate(
            SqlExpression instance,
            MethodInfo method,
            IReadOnlyList<SqlExpression> arguments,
            IDiagnosticsLogger<DbLoggerCategory.Query> logger)
        {
            if (method == _getCurrentInstant)
            {
                return SqlExpressionFactory.Function(
                    "strftime",
                    _getCurrentInstantArgs,
                    nullable: false,
                    Array.Empty<bool>(),
                    method.ReturnType);
            }

            var declaringType = method.DeclaringType;
            if (declaringType == typeof(LocalDateTime))
            {
                var modifiers = GetModifiers(method.Name, arguments[0]);
                // Unfortunately we can't use the datetime convenience function if we want to support fractional seconds :(
                return SqlExpressionFactory.Strftime(method.ReturnType, Constants.DateTimeFormat, instance, modifiers);
            }

            if (declaringType == typeof(LocalTime))
            {
                var modifiers = GetModifiers(method.Name, arguments[0]);
                // Unfortunately we can't use the time convenience function if we want to support fractional seconds :(
                return SqlExpressionFactory.Strftime(method.ReturnType, Constants.TimeFormat, instance, modifiers);
            }

            if (declaringType == typeof(LocalDate))
            {
                var modifiers = GetModifiers(method.Name, arguments[0]);
                return SqlExpressionFactory.Date(method.ReturnType, instance, modifiers);
            }

            return null;
        }

        private IEnumerable<SqlExpression>? GetModifiers(string methodName, SqlExpression argument)
        {
            return methodName switch
            {
                nameof(LocalDate.PlusYears) => Plus<int>(argument, Period.FromYears),
                nameof(LocalDate.PlusMonths) => Plus<int>(argument, Period.FromMonths),
                nameof(LocalDate.PlusWeeks) => Plus<int>(argument, Period.FromWeeks),
                nameof(LocalDate.PlusDays) => Plus<int>(argument, Period.FromDays),

                nameof(LocalTime.PlusHours) => Plus<long>(argument, Period.FromHours),
                nameof(LocalTime.PlusMinutes) => Plus<long>(argument, Period.FromMinutes),
                nameof(LocalTime.PlusSeconds) => Plus<long>(argument, Period.FromSeconds),
                nameof(LocalTime.PlusMilliseconds) => Plus<long>(argument, Period.FromMilliseconds),

                _ => null,
            };
        }

        private IEnumerable<SqlExpression> Plus<T>(SqlExpression argument, Func<T, Period> getPeriod)
        {
            if (argument is not SqlConstantExpression constant)
            {
                yield break;
            }

            var period = getPeriod((T)constant.Value).Normalize();

            if (period.Years != 0)
            {
                yield return GetModifier(period.Years, "years");
            }

            if (period.Months != 0)
            {
                yield return GetModifier(period.Months, "months");
            }

            if (period.Days != 0)
            {
                yield return GetModifier(period.Days, "days");
            }

            if (period.Hours != 0)
            {
                yield return GetModifier(period.Hours, "hours");
            }

            if (period.Minutes != 0)
            {
                yield return GetModifier(period.Minutes, "minutes");
            }

            if (period.Milliseconds != 0)
            {
                var fractionalSeconds = period.Seconds + ((double)period.Milliseconds / 1000);
                yield return GetModifier(fractionalSeconds, "seconds");
                yield break;
            }

            if (period.Seconds != 0)
            {
                yield return GetModifier(period.Seconds, "seconds");
            }
        }

        private SqlExpression GetModifier(long value, string unit)
            => SqlExpressionFactory.Constant($"{value:+##;-##;+0} {unit}");

        private SqlExpression GetModifier(double value, string unit)
            => SqlExpressionFactory.Constant($"{value:+#0.###;-##.###;+0} {unit}");
    }
}
