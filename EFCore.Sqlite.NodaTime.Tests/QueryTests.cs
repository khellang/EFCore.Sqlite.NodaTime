using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NodaTime;
using VerifyTests.EntityFramework;
using VerifyXunit;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    [UsesVerify]
    public abstract class QueryTests : IDisposable
    {
        protected QueryTests()
        {
            Db = new NodaTimeContext();
            Db.Database.EnsureCreated();
        }

        protected NodaTimeContext Db { get; }

        protected Task RunUpdate(Action<NodaTimeTypes> mutator,
            [CallerFilePath] string sourceFile = "")
        {
            SqlRecording.StartRecording();
            mutator(Db.NodaTimeTypes.Single());
            Db.SaveChanges();
            return Verifier.Verify(
                SqlRecording.FinishRecording(),
                sourceFile: sourceFile);
        }

        public void Dispose()
        {
            Db.Database.EnsureDeleted();
            Db.Dispose();
        }

        public class InstantQueryTests : QueryTests
        {
            public static readonly Instant Value = LocalDateTimeQueryTests.Value.InUtc().ToInstant();

            [Fact]
            public void Roundtrip()
            {
                Assert.Equal(Value, Db.NodaTimeTypes.Single().Instant);
            }

            [Fact]
            public Task GetCurrentInstant_From_Instance()
            {
                SqlRecording.StartRecording();
                _ = Db.NodaTimeTypes.Single(x => x.Instant < SystemClock.Instance.GetCurrentInstant());
                return Verifier.Verify(SqlRecording.FinishRecording());
            }
        }

        public class LocalDateTimeQueryTests : QueryTests
        {
            public static readonly LocalDateTime Value = new(2020, 10, 10, 23, 42, 16, 321);

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

        public class LocalDateQueryTests : QueryTests
        {
            public static readonly LocalDate Value = LocalDateTimeQueryTests.Value.Date;

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

        public class LocalTimeQueryTests : QueryTests
        {
            public static readonly LocalTime Value = LocalDateTimeQueryTests.Value.TimeOfDay;

            [Fact]
            public void Roundtrip()
            {
                Assert.Equal(Value, Db.NodaTimeTypes.Single().LocalTime);
            }

            [Fact]
            public Task Select_Equal()
            {
                SqlRecording.StartRecording();
                _ = Db.NodaTimeTypes.Single(x => x.LocalTime == new LocalTime(23, 42, 16, 321));
                return Verifier.Verify(SqlRecording.FinishRecording());
            }

            [Fact]
            public Task Select_GreaterThan()
            {
                SqlRecording.StartRecording();
                _ = Db.NodaTimeTypes.Single(x => x.LocalTime > new LocalTime(23, 42, 00));
                return Verifier.Verify(SqlRecording.FinishRecording());
            }

            [Fact]
            public Task Select_LessThan()
            {
                SqlRecording.StartRecording();
                _ = Db.NodaTimeTypes.Single(x => x.LocalTime < new LocalTime(23, 50, 00));
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
                _ = Db.NodaTimeTypes.Single(x => x.LocalTime.Hour == 23);
                return Verifier.Verify(SqlRecording.FinishRecording());
            }

            [Fact]
            public Task Select_Minute()
            {
                SqlRecording.StartRecording();
                _ = Db.NodaTimeTypes.Single(x => x.LocalTime.Minute == 42);
                return Verifier.Verify(SqlRecording.FinishRecording());
            }

            [Fact]
            public Task Select_Second()
            {
                SqlRecording.StartRecording();
                _ = Db.NodaTimeTypes.Single(x => x.LocalTime.Second == 16);
                return Verifier.Verify(SqlRecording.FinishRecording());
            }
        }

        public class LocalDateMethodQueryTests : QueryTests
        {
            [Fact]
            public Task PlusYears()
            {
                SqlRecording.StartRecording();
                var value = Db.NodaTimeTypes.Select(x => x.LocalDate.PlusYears(2)).Single();
                return Verifier.Verify(value);
            }

            [Fact]
            public Task PlusMonths()
            {
                SqlRecording.StartRecording();
                var value = Db.NodaTimeTypes.Select(x => x.LocalDate.PlusMonths(2)).Single();
                return Verifier.Verify(value);
            }

            [Fact]
            public Task PlusWeeks()
            {
                SqlRecording.StartRecording();
                var value = Db.NodaTimeTypes.Select(x => x.LocalDate.PlusWeeks(2)).Single();
                return Verifier.Verify(value);
            }

            [Fact]
            public Task PlusDays()
            {
                SqlRecording.StartRecording();
                var value = Db.NodaTimeTypes.Select(x => x.LocalDate.PlusDays(2)).Single();
                return Verifier.Verify(value);
            }
        }

        public class LocalTimeMethodQueryTests : QueryTests
        {
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
        }
    }
}
