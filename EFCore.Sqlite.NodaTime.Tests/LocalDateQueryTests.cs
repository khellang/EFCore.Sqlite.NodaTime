using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public class LocalDateQueryTests : QueryTests<LocalDate>
    {
        public static readonly LocalDate Value = LocalDateTimeQueryTests.Value.Date;

        public LocalDateQueryTests() : base(x => x.LocalDate)
        {
        }

        [Fact]
        public void Roundtrip() => Assert.Equal(Value, Query.Single());

        [Fact]
        public Task Equal() => Verify(x => x == new LocalDate(2020, 10, 10));

        [Fact]
        public Task GreaterThan() => Verify(x => x > new LocalDate(2020, 09, 10));

        [Fact]
        public Task LessThan() => Verify(x => x < new LocalDate(2020, 12, 13));

        [Fact]
        public Task Update() => RunUpdate(x => x.LocalDate = x.LocalDate.PlusDays(2));

        public class Properties : QueryTests<LocalDate>
        {
            public Properties() : base(x => x.LocalDate)
            {
            }

            [Fact]
            public Task Year() => Verify(x => x.Year == 2020);

            [Fact]
            public Task Month() => Verify(x => x.Month == 10);

            [Fact]
            public Task Day() => Verify(x => x.Day == 10);

            [Fact]
            public Task DayOfYear() => Verify(x => x.DayOfYear == 284);

            [Fact]
            public Task DayOfWeek() => Verify(x => x.DayOfWeek == IsoDayOfWeek.Saturday);
        }

        public class Methods : MethodQueryTests<LocalDate>
        {
            public Methods() : base(x => x.LocalDate)
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
}
