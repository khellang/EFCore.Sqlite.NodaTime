using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class SqliteInstantValueConverter : ValueConverter<Instant, string>
    {
        private static readonly InstantPattern _pattern =
            InstantPattern.CreateWithInvariantCulture("uuuu'-'MM'-'dd' 'HH':'mm':'ss'.'FFFFFFFFF");

        public static readonly SqliteInstantValueConverter Instance = new(_pattern);

        private SqliteInstantValueConverter(IPattern<Instant> pattern)
            : base(i => pattern.Format(i), t => pattern.Parse(t).GetValueOrThrow())
        {
        }
    }
}
