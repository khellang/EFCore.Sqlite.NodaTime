using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;

internal class SqliteLocalDateTimeTypeMapping() : SqliteNodaTimeTypeMapping<LocalDateTime>(SqlitePatterns.LocalDateTime)
{
    public static readonly SqliteLocalDateTimeTypeMapping Default = new();
}
