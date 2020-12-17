using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal.Converters
{
    public class SqliteLocalDateValueConverter : SqliteValueConverter<LocalDate>
    {
        public static readonly SqliteLocalDateValueConverter Instance = new();

        private SqliteLocalDateValueConverter() : base(LocalDatePattern.Iso)
        {
        }
    }
}
