using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal.Converters
{
    public class SqliteLocalDateTimeValueConverter : SqliteValueConverter<LocalDateTime>
    {
        private static readonly LocalDateTimePattern _pattern = LocalDateTimePattern
            .CreateWithInvariantCulture("uuuu'-'MM'-'dd' 'HH':'mm':'ss'.'FFFFFFFFF");

        public static readonly SqliteLocalDateTimeValueConverter Instance = new();

        private SqliteLocalDateTimeValueConverter() : base(_pattern)
        {
        }
    }
}
