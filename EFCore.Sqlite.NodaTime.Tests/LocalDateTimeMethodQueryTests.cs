using System.Threading.Tasks;
using NodaTime;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public class LocalDateTimeMethodQueryTests : MethodQueryTests<LocalDateTime>
    {
        public LocalDateTimeMethodQueryTests() : base(x => x.LocalDateTime)
        {
        }

        [Fact]
        public Task PlusYears() => Verify(x => x.PlusYears(2));

        [Fact]
        public Task PlusMonths() => Verify(x => x.PlusMonths(2));

        [Fact]
        public Task PlusWeeks() => Verify(x => x.PlusWeeks(2));

        [Fact]
        public Task PlusDays() => Verify(x => x.PlusDays(2));

        [Fact]
        public Task PlusHours() => Verify(x => x.PlusHours(2));

        [Fact]
        public Task PlusMinutes() => Verify(x => x.PlusMinutes(2));

        [Fact]
        public Task PlusSeconds() => Verify(x => x.PlusSeconds(2));

        [Fact]
        public Task PlusMilliseconds() => Verify(x => x.PlusMilliseconds(2));

        [Fact]
        public Task Combination() => Verify(x => x.PlusMonths(2).PlusDays(2).PlusHours(2).PlusSeconds(2));
    }
}
