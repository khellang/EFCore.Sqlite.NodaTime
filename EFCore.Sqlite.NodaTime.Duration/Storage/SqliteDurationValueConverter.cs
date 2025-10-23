using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage;

/// <summary>
/// A value converter for NodaTime <see cref="Duration"/> that
/// stores the value as a string using the <see cref="DurationPattern.JsonRoundtrip"/> pattern.
/// </summary>
public class SqliteDurationValueConverter : ValueConverter<Duration, string>
{
    public SqliteDurationValueConverter()
        : base(
            d => _pattern.Format(d),
            s => _pattern.Parse(s).GetValueOrThrow()) {}

    private static readonly DurationPattern _pattern = DurationPattern.JsonRoundtrip;
}
