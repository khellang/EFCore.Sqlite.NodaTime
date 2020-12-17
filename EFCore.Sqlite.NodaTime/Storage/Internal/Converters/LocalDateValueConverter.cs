using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class LocalDateValueConverter : ValueConverter<LocalDate, string>
    {
        public static readonly LocalDateValueConverter Instance = new(LocalDatePattern.Iso);

        private LocalDateValueConverter(IPattern<LocalDate> pattern)
            : base(d => pattern.Format(d), t => pattern.Parse(t).GetValueOrThrow())
        {
        }
    }
}
