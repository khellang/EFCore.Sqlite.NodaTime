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
        public void Roundtrip()
        {
            Assert.Equal(Value, Query.Single());
        }

        [Fact]
        public Task Select_Equal()
        {
            return Verify(x => x == new LocalDate(2020, 10, 10));
        }

        [Fact]
        public Task Select_GreaterThan()
        {
            return Verify(x => x > new LocalDate(2020, 09, 10));
        }

        [Fact]
        public Task Select_LessThan()
        {
            return Verify(x => x < new LocalDate(2020, 12, 13));
        }

        [Fact]
        public Task Update()
        {
            return RunUpdate(x => x.LocalDate = x.LocalDate.PlusDays(2));
        }

        [Fact]
        public Task Select_Year()
        {
            return Verify(x => x.Year == 2020);
        }

        [Fact]
        public Task Select_Month()
        {
            return Verify(x => x.Month == 10);
        }

        [Fact]
        public Task Select_Day()
        {
            return Verify(x => x.Day == 10);
        }

        [Fact]
        public Task Select_DayOfYear()
        {
            return Verify(x => x.DayOfYear == 284);
        }

        [Fact]
        public Task Select_DayOfWeek()
        {
            return Verify(x => x.DayOfWeek == IsoDayOfWeek.Saturday);
        }
    }
}
