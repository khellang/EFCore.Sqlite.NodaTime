using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using VerifyTests.EntityFramework;
using VerifyXunit;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public class LocalDateTimeMethodQueryTests : QueryTests<LocalDateTime>
    {
        public LocalDateTimeMethodQueryTests() : base(x => x.LocalDateTime)
        {
        }

        [Fact]
        public Task PlusYears()
        {
            SqlRecording.StartRecording();
            var value = Query.Select(x => x.PlusYears(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task PlusMonths()
        {
            SqlRecording.StartRecording();
            var value = Query.Select(x => x.PlusMonths(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task PlusWeeks()
        {
            SqlRecording.StartRecording();
            var value = Query.Select(x => x.PlusWeeks(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task PlusDays()
        {
            SqlRecording.StartRecording();
            var value = Query.Select(x => x.PlusDays(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task PlusHours()
        {
            SqlRecording.StartRecording();
            var value = Query.Select(x => x.PlusHours(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task PlusMinutes()
        {
            SqlRecording.StartRecording();
            var value = Query.Select(x => x.PlusMinutes(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task PlusSeconds()
        {
            SqlRecording.StartRecording();
            var value = Query.Select(x => x.PlusSeconds(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task PlusMilliseconds()
        {
            SqlRecording.StartRecording();
            var value = Query.Select(x => x.PlusMilliseconds(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task Combination()
        {
            SqlRecording.StartRecording();
            var value = Query.Select(x => x.PlusMonths(2).PlusDays(2).PlusHours(2).PlusSeconds(2)).Single();
            return Verifier.Verify(value);
        }
    }
}
