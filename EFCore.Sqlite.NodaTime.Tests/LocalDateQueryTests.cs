using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using VerifyTests.EntityFramework;
using VerifyXunit;
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
        public void Roundtrip()
        {
            Assert.Equal(Value, Db.NodaTimeTypes.Single().LocalDate);
        }

        [Fact]
        public Task Select_Equal()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDate == new LocalDate(2020, 10, 10));
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_GreaterThan()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDate > new LocalDate(2020, 09, 10));
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_LessThan()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDate < new LocalDate(2020, 12, 13));
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Update()
        {
            return RunUpdate(x => x.LocalDate = x.LocalDate.PlusDays(2));
        }

        [Fact]
        public Task Select_Year()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDate.Year == 2020);
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_Month()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDate.Month == 10);
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_Day()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDate.Day == 10);
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_DayOfYear()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDate.DayOfYear == 284);
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_DayOfWeek()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDate.DayOfWeek == IsoDayOfWeek.Saturday);
            return Verifier.Verify(SqlRecording.FinishRecording());
        }
    }
}