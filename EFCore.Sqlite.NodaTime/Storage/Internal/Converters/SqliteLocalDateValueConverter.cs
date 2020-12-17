using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal.Converters
{
    public class SqliteLocalDateValueConverter : ValueConverter<LocalDate, string>
    {
        public static readonly SqliteLocalDateValueConverter Instance = new(LocalDatePattern.Iso);

        private SqliteLocalDateValueConverter(IPattern<LocalDate> pattern)
            : base(d => pattern.Format(d), t => pattern.Parse(t).GetValueOrThrow())
        {
        }
    }
}
