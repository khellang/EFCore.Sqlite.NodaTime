using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite;

public class LocalTimeQueryTests : QueryTests<LocalTime>
{
    public static readonly LocalTime Value = LocalDateTimeQueryTests.Value.TimeOfDay;

    public LocalTimeQueryTests() : base(x => x.LocalTime)
    {
    }

    [Fact]
    public void Roundtrip() => Assert.Equal(Value, Query.Single());

    [Fact]
    public Task Equal() => VerifyQuery(x => x == new LocalTime(23, 42, 16, 321));

    [Fact]
    public Task GreaterThan() => VerifyQuery(x => x > new LocalTime(23, 42, 00));

    [Fact]
    public Task LessThan() => VerifyQuery(x => x < new LocalTime(23, 50, 00));

    [Fact]
    public Task Update() => VerifyUpdate(x => x.LocalTime = x.LocalTime.PlusSeconds(10));

    public class Properties : QueryTests<LocalTime>
    {
        public Properties() : base(x => x.LocalTime)
        {
        }

        [Fact]
        public Task Hour() => VerifyQuery(x => x.Hour == 23);

        [Fact]
        public Task Minute() => VerifyQuery(x => x.Minute == 42);

        [Fact]
        public Task Second() => VerifyQuery(x => x.Second == 16);
    }

    public class Methods : MethodQueryTests<LocalTime>
    {
        public Methods() : base(x => x.LocalTime)
        {
        }

        [Fact]
        public Task PlusHours() => VerifyMethod(x => x.PlusHours(2));

        [Fact]
        public Task PlusMinutes() => VerifyMethod(x => x.PlusMinutes(2));

        [Fact]
        public Task PlusSeconds() => VerifyMethod(x => x.PlusSeconds(2));

        [Fact]
        public Task PlusMilliseconds() => VerifyMethod(x => x.PlusMilliseconds(2));

        [Fact]
        public Task Combination() => VerifyMethod(x => x.PlusHours(2).PlusSeconds(2));

        [Fact]
        public Task ToTimeOnly() => VerifyMethod(x => x.ToTimeOnly());
    }
}