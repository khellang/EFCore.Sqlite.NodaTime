using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using VerifyTests.EntityFramework;
using VerifyXunit;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public class LocalDateMethodQueryTests : QueryTests<LocalDate>
    {
        public LocalDateMethodQueryTests() : base(x => x.LocalDate)
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
        public Task Combination()
        {
            SqlRecording.StartRecording();
            var value = Query.Select(x => x.PlusMonths(2).PlusDays(2)).Single();
            return Verifier.Verify(value);
        }
    }
}
