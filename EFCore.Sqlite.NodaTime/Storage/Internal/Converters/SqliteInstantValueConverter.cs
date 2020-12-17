using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal.Converters
{
    public class SqliteInstantValueConverter : SqliteValueConverter<Instant>
    {
        private static readonly InstantPattern _pattern = InstantPattern
            .CreateWithInvariantCulture("uuuu'-'MM'-'dd' 'HH':'mm':'ss'.'FFFFFFFFF");

        public static readonly SqliteInstantValueConverter Instance = new();

        private SqliteInstantValueConverter() : base(_pattern)
        {
        }
    }
}
