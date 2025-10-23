using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;

internal class SqliteLocalDateTypeMapping() : SqliteNodaTimeTypeMapping<LocalDate>(SqlitePatterns.LocalDate)
{
    public static readonly SqliteLocalDateTypeMapping Default = new();
}
