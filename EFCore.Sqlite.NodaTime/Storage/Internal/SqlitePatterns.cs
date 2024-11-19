using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;

internal static class SqlitePatterns
{
    private const string DateTimePattern = "uuuu-MM-dd HH:mm:ss.FFFFFFFFF";

    internal static readonly IPattern<LocalDateTime> LocalDateTime = LocalDateTimePattern.CreateWithInvariantCulture(DateTimePattern);

    internal static readonly IPattern<Instant> Instant = InstantPattern.CreateWithInvariantCulture(DateTimePattern);

    internal static readonly IPattern<LocalTime> LocalTime = LocalTimePattern.ExtendedIso;

    internal static readonly IPattern<LocalDate> LocalDate = LocalDatePattern.Iso;
}
