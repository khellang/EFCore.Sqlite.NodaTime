using System;
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
        public Task Equal() => VerifyQuery(x => x == new LocalDate(2020, 10, 10));

        [Fact]
        public Task GreaterThan() => VerifyQuery(x => x > new LocalDate(2020, 09, 10));

        [Fact]
        public Task LessThan() => VerifyQuery(x => x < new LocalDate(2020, 12, 13));

        [Fact]
        public Task Update() => VerifyUpdate(x => x.LocalDate = x.LocalDate.PlusDays(2));

        [Fact]
        public Task ToDateTimeUnspecified() => VerifyQuery(x => x.ToDateTimeUnspecified() > DateTime.Now);

        public class Properties : QueryTests<LocalDate>
        {
            public Properties() : base(x => x.LocalDate)
            {
            }

            [Fact]
            public Task Year() => VerifyQuery(x => x.Year == 2020);

            [Fact]
            public Task Month() => VerifyQuery(x => x.Month == 10);

            [Fact]
            public Task Day() => VerifyQuery(x => x.Day == 10);

            [Fact]
            public Task DayOfYear() => VerifyQuery(x => x.DayOfYear == 284);

            [Fact]
            public Task DayOfWeek() => VerifyQuery(x => x.DayOfWeek == IsoDayOfWeek.Saturday);
        }

        public class Methods : MethodQueryTests<LocalDate>
        {
            public Methods() : base(x => x.LocalDate)
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
            public Task Combination() => VerifyMethod(x => x.PlusMonths(2).PlusDays(2));
        }
    }
}
