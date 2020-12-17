using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal.Converters
{
    public class LocalTimeValueConverter : ValueConverter<LocalTime, string>
    {
        public static readonly LocalTimeValueConverter Instance = new(LocalTimePattern.ExtendedIso);

        private LocalTimeValueConverter(IPattern<LocalTime> pattern)
            : base(d => pattern.Format(d), t => pattern.Parse(t).GetValueOrThrow())
        {
        }
    }
}
