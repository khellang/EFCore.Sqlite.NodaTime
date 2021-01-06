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
            var value = Db.NodaTimeTypes.Select(x => x.LocalDateTime.PlusYears(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task PlusMonths()
        {
            SqlRecording.StartRecording();
            var value = Db.NodaTimeTypes.Select(x => x.LocalDateTime.PlusMonths(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task PlusWeeks()
        {
            SqlRecording.StartRecording();
            var value = Db.NodaTimeTypes.Select(x => x.LocalDateTime.PlusWeeks(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task PlusDays()
        {
            SqlRecording.StartRecording();
            var value = Db.NodaTimeTypes.Select(x => x.LocalDateTime.PlusDays(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task PlusHours()
        {
            SqlRecording.StartRecording();
            var value = Db.NodaTimeTypes.Select(x => x.LocalDateTime.PlusHours(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task PlusMinutes()
        {
            SqlRecording.StartRecording();
            var value = Db.NodaTimeTypes.Select(x => x.LocalDateTime.PlusMinutes(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task PlusSeconds()
        {
            SqlRecording.StartRecording();
            var value = Db.NodaTimeTypes.Select(x => x.LocalDateTime.PlusSeconds(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task PlusMilliseconds()
        {
            SqlRecording.StartRecording();
            var value = Db.NodaTimeTypes.Select(x => x.LocalDateTime.PlusMilliseconds(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task Combination()
        {
            SqlRecording.StartRecording();
            var value = Db.NodaTimeTypes.Select(x => x.LocalDateTime.PlusMonths(2).PlusDays(2).PlusHours(2).PlusSeconds(2)).Single();
            return Verifier.Verify(value);
        }
    }
}