using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;

internal class SqliteLocalDateTypeMapping() : SqliteTypeMapping<LocalDate>(SqlitePatterns.LocalDate)
{
    public static readonly SqliteLocalDateTypeMapping Default = new();
}
