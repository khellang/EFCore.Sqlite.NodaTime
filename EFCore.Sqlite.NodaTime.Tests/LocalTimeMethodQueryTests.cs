using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using VerifyTests.EntityFramework;
using VerifyXunit;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public class LocalTimeMethodQueryTests : QueryTests<LocalTime>
    {
        public LocalTimeMethodQueryTests() : base(x => x.LocalTime)
        {
        }

        [Fact]
        public Task PlusHours()
        {
            SqlRecording.StartRecording();
            var value = Db.NodaTimeTypes.Select(x => x.LocalTime.PlusHours(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task PlusMinutes()
        {
            SqlRecording.StartRecording();
            var value = Db.NodaTimeTypes.Select(x => x.LocalTime.PlusMinutes(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task PlusSeconds()
        {
            SqlRecording.StartRecording();
            var value = Db.NodaTimeTypes.Select(x => x.LocalTime.PlusSeconds(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task PlusMilliseconds()
        {
            SqlRecording.StartRecording();
            var value = Db.NodaTimeTypes.Select(x => x.LocalTime.PlusMilliseconds(2)).Single();
            return Verifier.Verify(value);
        }

        [Fact]
        public Task Combination()
        {
            SqlRecording.StartRecording();
            var value = Db.NodaTimeTypes.Select(x => x.LocalTime.PlusHours(2).PlusSeconds(2)).Single();
            return Verifier.Verify(value);
        }
    }
}