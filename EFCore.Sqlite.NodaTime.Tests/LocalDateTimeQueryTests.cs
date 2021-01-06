using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public class LocalDateTimeQueryTests : QueryTests<LocalDateTime>
    {
        public static readonly LocalDateTime Value = new(2020, 10, 10, 23, 42, 16, 321);

        public LocalDateTimeQueryTests() : base(x => x.LocalDateTime)
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
            return Verify(x => x == new LocalDateTime(2020, 10, 10, 23, 42, 16, 321));
        }

        [Fact]
        public Task Select_GreaterThan()
        {
            return Verify(x => x > new LocalDateTime(2020, 10, 10, 23, 42, 16, 200));
        }

        [Fact]
        public Task Select_LessThan()
        {
            return Verify(x => x < new LocalDateTime(2020, 10, 10, 23, 42, 16, 500));
        }

        [Fact]
        public Task Update()
        {
            return RunUpdate(x => x.LocalDateTime = x.LocalDateTime.PlusDays(2));
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
        public Task Select_Hour()
        {
            return Verify(x => x.Hour == 23);
        }

        [Fact]
        public Task Select_Minute()
        {
            return Verify(x => x.Minute == 42);
        }

        [Fact]
        public Task Select_Second()
        {
            return Verify(x => x.Second == 16);
        }

        [Fact]
        public Task Select_DayOfYear()
        {
            return Verify(x => x.DayOfYear == 284);
        }

        [Fact]
        public Task Select_Date()
        {
            return Verify(x => x.Date == new LocalDate(2020, 10, 10));
        }

        [Fact]
        public Task Select_TimeOfDay()
        {
            return Verify(x => x.TimeOfDay == new LocalTime(23, 42, 16, 321));
        }

        [Fact]
        public Task Select_DayOfWeek()
        {
            return Verify(x => x.DayOfWeek == IsoDayOfWeek.Saturday);
        }
    }
}
