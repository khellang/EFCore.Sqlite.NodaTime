using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;

internal class SqliteInstantTypeMapping() : SqliteTypeMapping<Instant>(SqlitePatterns.Instant)
{
    public static readonly SqliteInstantTypeMapping Default = new();
}
