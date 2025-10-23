

// ReSharper disable StringLiteralTypo

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators.Internal;

internal class DurationConstants
{
    public const double NanosecondsPerMillisecond =    1_000_000;
    public const double NanosecondsPerSecond =     1_000_000_000;
    public const double NanosecondsPerMinute =    60_000_000_000;
    public const double NanosecondsPerHour =    3600_000_000_000;
    public const double NanosecondsPerDay =    86400_000_000_000;

    public const double MillisecondsPerSecond = 1000;
    public const double SecondsPerMinute = 60;
    public const double SecondsPerHour = 3600;
    public const double SecondsPerDay = 86400;
}
