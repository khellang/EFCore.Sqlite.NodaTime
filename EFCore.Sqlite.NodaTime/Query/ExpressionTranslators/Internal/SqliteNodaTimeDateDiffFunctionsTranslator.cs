using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Sqlite.Extensions;

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators.Internal
{
    public class SqliteNodaTimeDateDiffFunctionsTranslator : IMethodCallTranslator
    {
        public SqliteNodaTimeDateDiffFunctionsTranslator(ISqlExpressionFactory sqlExpressionFactory)
        {
            SqlExpressionFactory = sqlExpressionFactory;
        }

        private ISqlExpressionFactory SqlExpressionFactory { get; }

        public SqlExpression? Translate(
            SqlExpression? instance,
            MethodInfo method,
            IReadOnlyList<SqlExpression> arguments,
            IDiagnosticsLogger<DbLoggerCategory.Query> logger)
        {
            if (!TryGetMultiplier(method, out var factory))
            {
                return null;
            }

            var startDate = arguments[1];
            var endDate = arguments[2];

            var typeMapping = ExpressionExtensions.InferTypeMapping(startDate, endDate);

            startDate = SqlExpressionFactory.ApplyTypeMapping(startDate, typeMapping);
            endDate = SqlExpressionFactory.ApplyTypeMapping(endDate, typeMapping);

            var fromDate = SqlExpressionFactory.JulianDay(startDate, method.ReturnType);
            var toDate = SqlExpressionFactory.JulianDay(endDate, method.ReturnType);

            return SqlExpressionFactory.Convert(factory(SqlExpressionFactory.Subtract(toDate, fromDate)), typeof(int));
        }

        private bool TryGetMultiplier(MemberInfo method, [NotNullWhen(true)] out Func<SqlExpression, SqlExpression>? factory)
        {
            if (method.DeclaringType == typeof(SqliteNodaTimeDbFunctionsExtensions))
            {
                switch (method.Name)
                {
                    case nameof(SqliteNodaTimeDbFunctionsExtensions.DateDiffYear):
                        factory = expression => SqlExpressionFactory
                            .Divide(expression, SqlExpressionFactory.Constant(365));
                        return true;

                    case nameof(SqliteNodaTimeDbFunctionsExtensions.DateDiffMonth):
                        factory = expression => SqlExpressionFactory
                            .Divide(expression, SqlExpressionFactory.Constant(30));
                        return true;

                    case nameof(SqliteNodaTimeDbFunctionsExtensions.DateDiffWeek):
                        factory = expression => SqlExpressionFactory
                            .Divide(expression, SqlExpressionFactory.Constant(7));
                        return true;

                    case nameof(SqliteNodaTimeDbFunctionsExtensions.DateDiffDay):
                        factory = expression => expression;
                        return true;

                    case nameof(SqliteNodaTimeDbFunctionsExtensions.DateDiffHour):
                        factory = expression => SqlExpressionFactory
                            .Multiply(expression, SqlExpressionFactory.Constant(24));
                        return true;

                    case nameof(SqliteNodaTimeDbFunctionsExtensions.DateDiffMinute):
                        factory = expression => SqlExpressionFactory
                            .Multiply(expression, SqlExpressionFactory.Constant(1440));
                        return true;

                    case nameof(SqliteNodaTimeDbFunctionsExtensions.DateDiffSecond):
                        factory = expression => SqlExpressionFactory
                            .Multiply(expression, SqlExpressionFactory.Constant(86400));
                        return true;

                    case nameof(SqliteNodaTimeDbFunctionsExtensions.DateDiffMillisecond):
                        factory = expression => SqlExpressionFactory
                            .Multiply(expression, SqlExpressionFactory.Constant(86400000));
                        return true;

                    default:
                        factory = default;
                        return false;
                }
            }

            factory = default;
            return false;
        }
    }
}
