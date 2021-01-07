using System.Threading.Tasks;
using NodaTime;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public class LocalDateMethodQueryTests : MethodQueryTests<LocalDate>
    {
        public LocalDateMethodQueryTests() : base(x => x.LocalDate)
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
        public Task Combination() => Verify(x => x.PlusMonths(2).PlusDays(2));
    }
}
