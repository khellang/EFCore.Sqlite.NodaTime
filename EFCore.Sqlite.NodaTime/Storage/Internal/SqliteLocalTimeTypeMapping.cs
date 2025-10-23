using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;

internal class SqliteLocalTimeTypeMapping() : SqliteNodaTimeTypeMapping<LocalTime>(SqlitePatterns.LocalTime)
{
    public static readonly SqliteLocalTimeTypeMapping Default = new();
}
