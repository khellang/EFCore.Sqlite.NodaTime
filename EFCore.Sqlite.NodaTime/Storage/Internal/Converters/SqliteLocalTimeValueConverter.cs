using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal.Converters
{
    public class SqliteLocalTimeValueConverter : ValueConverter<LocalTime, string>
    {
        public static readonly SqliteLocalTimeValueConverter Instance = new(LocalTimePattern.ExtendedIso);

        private SqliteLocalTimeValueConverter(IPattern<LocalTime> pattern)
            : base(d => pattern.Format(d), t => pattern.Parse(t).GetValueOrThrow())
        {
        }
    }
}
