using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite;

public class LocalDateTimeQueryTests : QueryTests<LocalDateTime>
{
    public static readonly LocalDateTime Value = new(2020, 10, 10, 23, 42, 16, 321);

    public LocalDateTimeQueryTests() : base(x => x.LocalDateTime)
    {
    }

    [Fact]
    public void Roundtrip() => Assert.Equal(Value, Query.Single());

    [Fact]
    public Task Equal() => VerifyQuery(x => x == new LocalDateTime(2020, 10, 10, 23, 42, 16, 321));

    [Fact]
    public Task GreaterThan() => VerifyQuery(x => x > new LocalDateTime(2020, 10, 10, 23, 42, 16, 200));

    [Fact]
    public Task LessThan() => VerifyQuery(x => x < new LocalDateTime(2020, 10, 10, 23, 42, 16, 500));

    [Fact]
    public Task Update() => VerifyUpdate(x => x.LocalDateTime = x.LocalDateTime.PlusDays(2));

    public class Properties : QueryTests<LocalDateTime>
    {
        public Properties() : base(x => x.LocalDateTime)
        {
        }

        [Fact]
        public Task Year() => VerifyQuery(x => x.Year == 2020);

        [Fact]
        public Task Month() => VerifyQuery(x => x.Month == 10);

        [Fact]
        public Task Day() => VerifyQuery(x => x.Day == 10);

        [Fact]
        public Task Hour() => VerifyQuery(x => x.Hour == 23);

        [Fact]
        public Task Minute() => VerifyQuery(x => x.Minute == 42);

        [Fact]
        public Task Second() => VerifyQuery(x => x.Second == 16);

        [Fact]
        public Task DayOfYear() => VerifyQuery(x => x.DayOfYear == 284);

        [Fact]
        public Task Date() => VerifyQuery(x => x.Date == new LocalDate(2020, 10, 10));

        [Fact]
        public Task TimeOfDay() => VerifyQuery(x => x.TimeOfDay == new LocalTime(23, 42, 16, 321));

        [Fact]
        public Task DayOfWeek() => VerifyQuery(x => x.DayOfWeek == IsoDayOfWeek.Saturday);
    }

    public class Methods : MethodQueryTests<LocalDateTime>
    {
        public Methods() : base(x => x.LocalDateTime)
        {
        }

        [Fact]
        public Task PlusYears() => VerifyMethod(x => x.PlusYears(2));

        [Fact]
        public Task PlusMonths() => VerifyMethod(x => x.PlusMonths(2));

        [Fact]
        public Task PlusWeeks() => VerifyMethod(x => x.PlusWeeks(2));

        [Fact]
        public Task PlusDays() => VerifyMethod(x => x.PlusDays(2));

        [Fact]
        public Task PlusHours() => VerifyMethod(x => x.PlusHours(2));

        [Fact]
        public Task PlusMinutes() => VerifyMethod(x => x.PlusMinutes(2));

        [Fact]
        public Task PlusSeconds() => VerifyMethod(x => x.PlusSeconds(2));

        [Fact]
        public Task PlusMilliseconds() => VerifyMethod(x => x.PlusMilliseconds(2));

        [Fact]
        public Task Combination() => VerifyMethod(x => x.PlusMonths(2).PlusDays(2).PlusHours(2).PlusSeconds(2));

        [Fact]
        public Task ToDateTimeUnspecified() => VerifyMethod(x => x.ToDateTimeUnspecified());
    }
}