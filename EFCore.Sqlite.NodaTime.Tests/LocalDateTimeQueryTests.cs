using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using VerifyTests.EntityFramework;
using VerifyXunit;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public class LocalDateTimeQueryTests : QueryTests<LocalDateTime>
    {
        public static readonly LocalDateTime Value = new(2020, 10, 10, 23, 42, 16, 321);

        public LocalDateTimeQueryTests() : base(x => x.LocalDateTime)
        {
        }

        [Fact]
        public void Roundtrip()
        {
            Assert.Equal(Value, Db.NodaTimeTypes.Single().LocalDateTime);
        }

        [Fact]
        public Task Select_Equal()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime == new LocalDateTime(2020, 10, 10, 23, 42, 16, 321));
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_GreaterThan()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime > new LocalDateTime(2020, 10, 10, 23, 42, 16, 200));
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_LessThan()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime < new LocalDateTime(2020, 10, 10, 23, 42, 16, 500));
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Update()
        {
            return RunUpdate(x => x.LocalDateTime = x.LocalDateTime.PlusDays(2));
        }

        [Fact]
        public Task Select_Year()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.Year == 2020);
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_Month()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.Month == 10);
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_Day()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.Day == 10);
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_Hour()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.Hour == 23);
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_Minute()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.Minute == 42);
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_Second()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.Second == 16);
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_DayOfYear()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.DayOfYear == 284);
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_Date()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.Date == new LocalDate(2020, 10, 10));
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_TimeOfDay()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.TimeOfDay == new LocalTime(23, 42, 16, 321));
            return Verifier.Verify(SqlRecording.FinishRecording());
        }

        [Fact]
        public Task Select_DayOfWeek()
        {
            SqlRecording.StartRecording();
            _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.DayOfWeek == IsoDayOfWeek.Saturday);
            return Verifier.Verify(SqlRecording.FinishRecording());
        }
    }
}