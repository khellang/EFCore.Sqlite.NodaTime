using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;

public class SqliteLocalDateTypeMapping() : SqliteTypeMapping<LocalDate>(SqlitePatterns.LocalDate)
{
    public static readonly SqliteLocalDateTypeMapping Default = new();
}
