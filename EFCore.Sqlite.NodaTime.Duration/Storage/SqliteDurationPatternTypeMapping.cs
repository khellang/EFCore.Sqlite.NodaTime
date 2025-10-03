using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage;

/// <summary>
/// A type mapping for NodaTime <see cref="NodaTime.Duration"/>
/// that stores the value as a nanoseconds long integer in SQLite.
/// </summary>
internal class SqliteDurationPatternTypeMapping : SqliteTypeMapping<Duration>
{
    public SqliteDurationPatternTypeMapping() : base(DurationPattern.JsonRoundtrip) { }

    public static readonly SqliteDurationPatternTypeMapping Default = new();
}
