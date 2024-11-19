using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Extensions
{
    internal static class NodaTimePatternExtensions
    {
        public static ValueConverter<T, string> AsValueConverter<T>(this IPattern<T> pattern)
            => new Converter<T>(pattern);

        private class Converter<T>(IPattern<T> pattern) : ValueConverter<T, string>(
            x => pattern.Format(x),
            x => pattern.Parse(x).GetValueOrThrow());
    }
}
