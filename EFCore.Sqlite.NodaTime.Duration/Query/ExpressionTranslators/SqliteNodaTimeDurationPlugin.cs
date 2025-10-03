using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Sqlite.Storage;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators;

internal class SqliteNodaTimeDurationPlugin :
    IMemberTranslatorPlugin,
    IMethodCallTranslatorPlugin,
    IRelationalTypeMappingSourcePlugin
{
    private readonly ISqlExpressionFactory _sqlExpressionFactory;
    private readonly ITypeMappingSource _typeMappingSource;

    public SqliteNodaTimeDurationPlugin(
        ISqlExpressionFactory sqlExpressionFactory,
        ITypeMappingSource typeMappingSource)
    {
        _sqlExpressionFactory = sqlExpressionFactory;
        _typeMappingSource = typeMappingSource;
    }

    IEnumerable<IMemberTranslator> IMemberTranslatorPlugin.Translators =>
    [
        new SqliteNodaTimeDurationMemberTranslator(_sqlExpressionFactory, _typeMappingSource)
    ];

    IEnumerable<IMethodCallTranslator> IMethodCallTranslatorPlugin.Translators =>
    [
        new SqliteNodaTimeDurationMethodCallTranslator(_sqlExpressionFactory, _typeMappingSource)
    ];

    public RelationalTypeMapping? FindMapping(in RelationalTypeMappingInfo mappingInfo) =>
        mappingInfo.ClrType == typeof(NodaTime.Duration) ? SqliteDurationSecondsTypeMapping.Default : null;
}
