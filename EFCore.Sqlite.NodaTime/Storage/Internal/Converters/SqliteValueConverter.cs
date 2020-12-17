using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal.Converters
{
    public abstract class SqliteValueConverter<T> : ValueConverter<T, string>
    {
        protected SqliteValueConverter(IPattern<T> pattern)
            : base(x => pattern.Format(x), x => pattern.Parse(x).GetValueOrThrow())
        {
        }
    }
}
