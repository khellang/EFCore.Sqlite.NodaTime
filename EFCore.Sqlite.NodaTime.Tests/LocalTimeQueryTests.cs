using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public class LocalTimeQueryTests : QueryTests<LocalTime>
    {
        public static readonly LocalTime Value = LocalDateTimeQueryTests.Value.TimeOfDay;

        public LocalTimeQueryTests() : base(x => x.LocalTime)
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
            return Verify(x => x == new LocalTime(23, 42, 16, 321));
        }

        [Fact]
        public Task Select_GreaterThan()
        {
            return Verify(x => x > new LocalTime(23, 42, 00));
        }

        [Fact]
        public Task Select_LessThan()
        {
            return Verify(x => x < new LocalTime(23, 50, 00));
        }

        [Fact]
        public Task Update()
        {
            return RunUpdate(x => x.LocalTime = x.LocalTime.PlusSeconds(10));
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
    }
}
