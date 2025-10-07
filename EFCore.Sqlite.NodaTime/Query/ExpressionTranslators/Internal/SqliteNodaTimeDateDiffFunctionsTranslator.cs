using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators.Internal;

internal class SqliteNodaTimeDateDiffFunctionsTranslator(ISqlExpressionFactory sqlExpressionFactory) : IMethodCallTranslator
{
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

        startDate = sqlExpressionFactory.ApplyTypeMapping(startDate, typeMapping);
        endDate = sqlExpressionFactory.ApplyTypeMapping(endDate, typeMapping);

        var fromDate = sqlExpressionFactory.JulianDay(startDate, method.ReturnType);
        var toDate = sqlExpressionFactory.JulianDay(endDate, method.ReturnType);

        return sqlExpressionFactory.Convert(factory(sqlExpressionFactory.Subtract(toDate, fromDate)), typeof(int));
    }

    private bool TryGetMultiplier(MemberInfo method, [NotNullWhen(true)] out Func<SqlExpression, SqlExpression>? factory)
    {
        if (method.DeclaringType == typeof(SqliteNodaTimeDbFunctionsExtensions))
        {
            switch (method.Name)
            {
                case nameof(SqliteNodaTimeDbFunctionsExtensions.DateDiffYear):
                    factory = expression => sqlExpressionFactory
                        .Divide(expression, sqlExpressionFactory.Constant(365));
                    return true;

                case nameof(SqliteNodaTimeDbFunctionsExtensions.DateDiffMonth):
                    factory = expression => sqlExpressionFactory
                        .Divide(expression, sqlExpressionFactory.Constant(30));
                    return true;

                case nameof(SqliteNodaTimeDbFunctionsExtensions.DateDiffWeek):
                    factory = expression => sqlExpressionFactory
                        .Divide(expression, sqlExpressionFactory.Constant(7));
                    return true;

                case nameof(SqliteNodaTimeDbFunctionsExtensions.DateDiffDay):
                    factory = expression => expression;
                    return true;

                case nameof(SqliteNodaTimeDbFunctionsExtensions.DateDiffHour):
                    factory = expression => sqlExpressionFactory
                        .Multiply(expression, sqlExpressionFactory.Constant(24));
                    return true;

                case nameof(SqliteNodaTimeDbFunctionsExtensions.DateDiffMinute):
                    factory = expression => sqlExpressionFactory
                        .Multiply(expression, sqlExpressionFactory.Constant(1440));
                    return true;

                case nameof(SqliteNodaTimeDbFunctionsExtensions.DateDiffSecond):
                    factory = expression => sqlExpressionFactory
                        .Multiply(expression, sqlExpressionFactory.Constant(86400));
                    return true;

                case nameof(SqliteNodaTimeDbFunctionsExtensions.DateDiffMillisecond):
                    factory = expression => sqlExpressionFactory
                        .Multiply(expression, sqlExpressionFactory.Constant(86400000));
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
