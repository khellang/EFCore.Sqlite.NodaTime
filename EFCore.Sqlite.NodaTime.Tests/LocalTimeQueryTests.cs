using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using VerifyTests.EntityFramework;
using VerifyXunit;
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
            SqlRecording.StartRecording();
            _ = Query.Single(x => x == new LocalTime(23, 42, 16, 321));
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_GreaterThan()
        {
            SqlRecording.StartRecording();
            _ = Query.Single(x => x > new LocalTime(23, 42, 00));
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_LessThan()
        {
            SqlRecording.StartRecording();
            _ = Query.Single(x => x < new LocalTime(23, 50, 00));
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Update()
        {
            return RunUpdate(x => x.LocalTime = x.LocalTime.PlusSeconds(10));
        }

        [Fact]
        public Task Select_Hour()
        {
            SqlRecording.StartRecording();
            _ = Query.Single(x => x.Hour == 23);
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_Minute()
        {
            SqlRecording.StartRecording();
            _ = Query.Single(x => x.Minute == 42);
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_Second()
        {
            SqlRecording.StartRecording();
            _ = Query.Single(x => x.Second == 16);
            return Verifier.Verify(SqlRecording.FinishRecording());
        }
    }
}
