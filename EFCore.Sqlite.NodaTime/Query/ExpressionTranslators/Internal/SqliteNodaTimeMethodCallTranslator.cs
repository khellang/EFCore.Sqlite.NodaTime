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
                SqlExpressionFactory.Constant("%Y-%m-%d %H:%M:%f"),
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
            if (declaringType == typeof(LocalDateTime) ||
                declaringType == typeof(LocalDate) ||
                declaringType == typeof(LocalTime))
            {
                return TranslateDateTime(instance, method, arguments);
            }

            return null;
        }

        private SqlExpression? TranslateDateTime(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments)
        {
            SqlExpression GetDateTime(IEnumerable<SqlExpression> modifiers)
                => SqlExpressionFactory.DateTime(method.ReturnType, instance, modifiers);

            SqlExpression PlusInt64(Func<long, Period> getPeriod)
                => GetDateTime(GetModifiers(arguments[0], getPeriod));

            SqlExpression PlusInt32(Func<int, Period> getPeriod)
                => GetDateTime(GetModifiers(arguments[0], x => getPeriod((int) x)));

            return method.Name switch
            {
                nameof(LocalDate.PlusYears) => PlusInt32(Period.FromYears),
                nameof(LocalDate.PlusMonths) => PlusInt32(Period.FromMonths),
                nameof(LocalDate.PlusWeeks) => PlusInt32(Period.FromWeeks),
                nameof(LocalDate.PlusDays) => PlusInt32(Period.FromDays),

                nameof(LocalTime.PlusHours) => PlusInt64(Period.FromHours),
                nameof(LocalTime.PlusMinutes) => PlusInt64(Period.FromMinutes),
                nameof(LocalTime.PlusSeconds) => PlusInt64(Period.FromSeconds),

                _ => null,
            };
        }

        private IEnumerable<SqlExpression> GetModifiers(SqlExpression argument, Func<long, Period> getPeriod)
        {
            if (argument is not SqlConstantExpression constant)
            {
                yield break;
            }

            var value = Convert.ToInt64(constant.Value);

            var period = getPeriod(value).Normalize();

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

            if (period.Seconds != 0)
            {
                yield return GetModifier(period.Seconds, "seconds");
            }
        }

        private SqlExpression GetModifier(long value, string unit)
            => SqlExpressionFactory.Constant($"{value:+#;-#;+0} {unit}");
    }
}
