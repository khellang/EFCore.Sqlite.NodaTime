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
        public void Roundtrip() => Assert.Equal(Value, Query.Single());

        [Fact]
        public Task Select_Equal() => Verify(x => x == new LocalDateTime(2020, 10, 10, 23, 42, 16, 321));

        [Fact]
        public Task Select_GreaterThan() => Verify(x => x > new LocalDateTime(2020, 10, 10, 23, 42, 16, 200));

        [Fact]
        public Task Select_LessThan() => Verify(x => x < new LocalDateTime(2020, 10, 10, 23, 42, 16, 500));

        [Fact]
        public Task Update() => RunUpdate(x => x.LocalDateTime = x.LocalDateTime.PlusDays(2));

        [Fact]
        public Task Select_Year() => Verify(x => x.Year == 2020);

        [Fact]
        public Task Select_Month() => Verify(x => x.Month == 10);

        [Fact]
        public Task Select_Day() => Verify(x => x.Day == 10);

        [Fact]
        public Task Select_Hour() => Verify(x => x.Hour == 23);

        [Fact]
        public Task Select_Minute() => Verify(x => x.Minute == 42);

        [Fact]
        public Task Select_Second() => Verify(x => x.Second == 16);

        [Fact]
        public Task Select_DayOfYear() => Verify(x => x.DayOfYear == 284);

        [Fact]
        public Task Select_Date() => Verify(x => x.Date == new LocalDate(2020, 10, 10));

        [Fact]
        public Task Select_TimeOfDay() => Verify(x => x.TimeOfDay == new LocalTime(23, 42, 16, 321));

        [Fact]
        public Task Select_DayOfWeek() => Verify(x => x.DayOfWeek == IsoDayOfWeek.Saturday);
    }
}
