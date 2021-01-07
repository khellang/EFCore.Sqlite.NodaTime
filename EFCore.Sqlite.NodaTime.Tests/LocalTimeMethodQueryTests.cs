using System.Threading.Tasks;
using NodaTime;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public class LocalTimeMethodQueryTests : MethodQueryTests<LocalTime>
    {
        public LocalTimeMethodQueryTests() : base(x => x.LocalTime)
        {
        }

        [Fact]
        public Task PlusHours() => Verify(x => x.PlusHours(2));

        [Fact]
        public Task PlusMinutes() => Verify(x => x.PlusMinutes(2));

        [Fact]
        public Task PlusSeconds() => Verify(x => x.PlusSeconds(2));

        [Fact]
        public Task PlusMilliseconds() => Verify(x => x.PlusMilliseconds(2));

        [Fact]
        public Task Combination() => Verify(x => x.PlusHours(2).PlusSeconds(2));
    }
}
