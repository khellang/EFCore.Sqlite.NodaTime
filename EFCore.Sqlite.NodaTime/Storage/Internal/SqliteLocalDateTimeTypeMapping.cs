using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;

public class SqliteLocalDateTimeTypeMapping() : SqliteTypeMapping<LocalDateTime>(SqlitePatterns.LocalDateTime)
{
    public static readonly SqliteLocalDateTimeTypeMapping Default = new();
}
