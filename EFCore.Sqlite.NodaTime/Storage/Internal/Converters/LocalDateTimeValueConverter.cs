using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class LocalDateTimeValueConverter : ValueConverter<LocalDateTime, string>
    {
        private static readonly LocalDateTimePattern _pattern =
            LocalDateTimePattern.CreateWithInvariantCulture("uuuu'-'MM'-'dd' 'HH':'mm':'ss'.'FFFFFFFFF");

        public static readonly LocalDateTimeValueConverter Instance = new(_pattern);

        private LocalDateTimeValueConverter(IPattern<LocalDateTime> pattern)
            : base(d => pattern.Format(d), t => pattern.Parse(t).GetValueOrThrow())
        {
        }
    }
}
