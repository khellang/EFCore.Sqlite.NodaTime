using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage;

internal class SqliteNodaTimeDurationTypeMappingSourcePlugin : IRelationalTypeMappingSourcePlugin
{
    public RelationalTypeMapping? FindMapping(in RelationalTypeMappingInfo mappingInfo) =>
        mappingInfo.ClrType == typeof(NodaTime.Duration) ? SqliteDurationSecondsTypeMapping.Default : null;
}
