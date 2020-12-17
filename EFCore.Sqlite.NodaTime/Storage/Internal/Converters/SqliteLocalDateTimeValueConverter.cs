using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal.Converters
{
    public class SqliteLocalDateTimeValueConverter : ValueConverter<LocalDateTime, string>
    {
        private static readonly LocalDateTimePattern _pattern =
            LocalDateTimePattern.CreateWithInvariantCulture("uuuu'-'MM'-'dd' 'HH':'mm':'ss'.'FFFFFFFFF");

        public static readonly SqliteLocalDateTimeValueConverter Instance = new(_pattern);

        private SqliteLocalDateTimeValueConverter(IPattern<LocalDateTime> pattern)
            : base(d => pattern.Format(d), t => pattern.Parse(t).GetValueOrThrow())
        {
        }
    }
}
