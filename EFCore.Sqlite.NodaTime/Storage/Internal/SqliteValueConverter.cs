using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public static class SqliteValueConverter
    {
        public static ValueConverter<T, string> Create<T>(IPattern<T> pattern)
        {
            return new Converter<T>(pattern);
        }

        private class Converter<T> : ValueConverter<T, string>
        {
            public Converter(IPattern<T> pattern)
                : base(x => pattern.Format(x), x => pattern.Parse(x).GetValueOrThrow())
            {
            }
        }
    }
}
