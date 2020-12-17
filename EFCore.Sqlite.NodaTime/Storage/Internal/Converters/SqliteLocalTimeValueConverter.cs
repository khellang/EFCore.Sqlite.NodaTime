using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal.Converters
{
    public class SqliteLocalTimeValueConverter : SqliteValueConverter<LocalTime>
    {
        public static readonly SqliteLocalTimeValueConverter Instance = new();

        private SqliteLocalTimeValueConverter() : base(LocalTimePattern.ExtendedIso)
        {
        }
    }
}
