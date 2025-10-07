using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators.Internal;

internal class SqliteNodaTimeDurationMemberTranslator : IMemberTranslator
{
    private readonly ISqlExpressionFactory _sqlExpressionFactory;
    private readonly RelationalTypeMapping _doubleTypeMapping;

    public SqliteNodaTimeDurationMemberTranslator(
        ISqlExpressionFactory sqlExpressionFactory,
        ITypeMappingSource typeMappingSource)
    {
        _sqlExpressionFactory = sqlExpressionFactory;

        _doubleTypeMapping = typeMappingSource.GetRelationalTypeMapping(typeof(double));
    }

    public SqlExpression? Translate(
        SqlExpression? instance,
        MemberInfo member,
        Type returnType,
        IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (instance != null && member.DeclaringType == typeof(NodaTime.Duration))
        {
            return TranslateDuration(instance, member);
        }

        return null;
    }

    private SqlExpression? TranslateDuration(SqlExpression instance, MemberInfo member)
    {
        // Input value is in seconds as double

        Debug.Assert(instance.Type == typeof(NodaTime.Duration));
        return member.Name switch
        {
            nameof(NodaTime.Duration.TotalNanoseconds) =>
                _sqlExpressionFactory.Multiply(
                    instance,
                    _sqlExpressionFactory.Constant(DurationConstants.NanosecondsPerSecond, _doubleTypeMapping),
                    _doubleTypeMapping),

            nameof(NodaTime.Duration.TotalMilliseconds) =>
                _sqlExpressionFactory.Multiply(
                    instance,
                    _sqlExpressionFactory.Constant(DurationConstants.MillisecondsPerSecond, _doubleTypeMapping),
                    _doubleTypeMapping),

            nameof(NodaTime.Duration.TotalSeconds) =>
                // Value is already in seconds, we just need to apply the correct type mapping
                _sqlExpressionFactory.ChangeExpressionType(instance, _doubleTypeMapping),

            nameof(NodaTime.Duration.TotalMinutes) =>
                _sqlExpressionFactory.Divide(
                    instance,
                    _sqlExpressionFactory.Constant(DurationConstants.SecondsPerMinute, _doubleTypeMapping),
                    _doubleTypeMapping),

            nameof(NodaTime.Duration.TotalHours) =>
                _sqlExpressionFactory.Divide(
                    instance,
                    _sqlExpressionFactory.Constant(DurationConstants.SecondsPerHour, _doubleTypeMapping),
                    _doubleTypeMapping),

            nameof(NodaTime.Duration.TotalDays) =>
                _sqlExpressionFactory.Divide(
                    instance,
                    _sqlExpressionFactory.Constant(DurationConstants.SecondsPerDay, _doubleTypeMapping),
                    _doubleTypeMapping),

            _ => null
        };
    }
}
