using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;

public class SqliteInstantTypeMapping() : SqliteTypeMapping<Instant>(SqlitePatterns.Instant)
{
    public static readonly SqliteInstantTypeMapping Default = new();
}
