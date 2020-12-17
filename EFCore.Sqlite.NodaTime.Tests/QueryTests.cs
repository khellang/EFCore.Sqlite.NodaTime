using System;
using System.Linq;
using Microsoft.Data.Sqlite;
using NodaTime;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public abstract class QueryTests : IDisposable
    {
        protected QueryTests()
        {
            Db = new NodaTimeContext();
            Db.Database.EnsureCreated();
        }

        protected NodaTimeContext Db { get; }

        private static string Condense(string str) =>
            string.Join(" ", str.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));

        protected void RunUpdate(Action<NodaTimeTypes> mutator)
        {
            mutator(Db.NodaTimeTypes.Single());
            Db.SaveChanges();
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
            public void GetCurrentInstant_From_Instance()
            {
                _ = Db.NodaTimeTypes.Single(x => x.Instant < SystemClock.Instance.GetCurrentInstant());
                Assert.Contains(@"WHERE ""n"".""Instant"" < strftime('%Y-%m-%d %H:%M:%f', 'now')", Db.Sql);
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
            public void Select_Equal()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime == new LocalDateTime(2020, 10, 10, 23, 42, 16, 321));
                Assert.Contains(@"WHERE ""n"".""LocalDateTime"" = '2020-10-10 23:42:16.321'", Db.Sql);
            }

            [Fact]
            public void Select_GreaterThan()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime > new LocalDateTime(2020, 10, 10, 23, 42, 16, 200));
                Assert.Contains(@"WHERE ""n"".""LocalDateTime"" > '2020-10-10 23:42:16.2'", Db.Sql);
            }

            [Fact]
            public void Select_LessThan()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime < new LocalDateTime(2020, 10, 10, 23, 42, 16, 500));
                Assert.Contains(@"WHERE ""n"".""LocalDateTime"" < '2020-10-10 23:42:16.5'", Db.Sql);
            }

            [Fact]
            public void Update()
            {
                RunUpdate(x => x.LocalDateTime = x.LocalDateTime.PlusDays(2));
                Assert.Contains(@"SET ""LocalDateTime"" = @p0", Db.Sql);
                Assert.Contains(@"p0='2020-10-12 23:42:16.321'", Db.Parameters);
            }

            [Fact]
            public void Select_Year()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.Year == 2020);
                Assert.Contains(@"WHERE CAST(strftime('%Y', ""n"".""LocalDateTime"") AS INTEGER) = 2020", Db.Sql);
            }

            [Fact]
            public void Select_Month()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.Month == 10);
                Assert.Contains(@"WHERE CAST(strftime('%m', ""n"".""LocalDateTime"") AS INTEGER) = 10", Db.Sql);
            }

            [Fact]
            public void Select_Day()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.Day == 10);
                Assert.Contains(@"WHERE CAST(strftime('%d', ""n"".""LocalDateTime"") AS INTEGER) = 10", Db.Sql);
            }

            [Fact]
            public void Select_Hour()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.Hour == 23);
                Assert.Contains(@"WHERE CAST(strftime('%H', ""n"".""LocalDateTime"") AS INTEGER) = 23", Db.Sql);
            }

            [Fact]
            public void Select_Minute()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.Minute == 42);
                Assert.Contains(@"WHERE CAST(strftime('%M', ""n"".""LocalDateTime"") AS INTEGER) = 42", Db.Sql);
            }

            [Fact]
            public void Select_Second()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.Second == 16);
                Assert.Contains(@"WHERE CAST(strftime('%S', ""n"".""LocalDateTime"") AS INTEGER) = 16", Db.Sql);
            }

            [Fact]
            public void Select_DayOfYear()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.DayOfYear == 284);
                Assert.Contains(@"WHERE CAST(strftime('%j', ""n"".""LocalDateTime"") AS INTEGER) = 284", Db.Sql);
            }

            [Fact]
            public void Select_Date()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.Date == new LocalDate(2020, 10, 10));
                Assert.Contains(@"WHERE date(""n"".""LocalDateTime"") = '2020-10-10'", Db.Sql);
            }

            [Fact]
            public void Select_TimeOfDay()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.TimeOfDay == new LocalTime(23, 42, 16, 321));
                Assert.Contains(@"WHERE CAST(strftime('%H:%M:%f', ""n"".""LocalDateTime"") AS TEXT) = '23:42:16.321'", Db.Sql);
            }

            [Fact]
            public void Select_DayOfWeek()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDateTime.DayOfWeek == IsoDayOfWeek.Saturday);
                Assert.Contains("WHERE CASE WHEN CAST(strftime('%w', \"n\".\"LocalDateTime\") AS INTEGER) = 0 THEN 7 " +
                                "ELSE CAST(strftime('%w', \"n\".\"LocalDateTime\") AS INTEGER) END = 6", Condense(Db.Sql));
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
            public void Select_Equal()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDate == new LocalDate(2020, 10, 10));
                Assert.Contains(@"WHERE ""n"".""LocalDate"" = '2020-10-10'", Db.Sql);
            }

            [Fact]
            public void Select_GreaterThan()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDate > new LocalDate(2020, 09, 10));
                Assert.Contains(@"WHERE ""n"".""LocalDate"" > '2020-09-10'", Db.Sql);
            }

            [Fact]
            public void Select_LessThan()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDate < new LocalDate(2020, 12, 13));
                Assert.Contains(@"WHERE ""n"".""LocalDate"" < '2020-12-13'", Db.Sql);
            }

            [Fact]
            public void Update()
            {
                RunUpdate(x => x.LocalDate = x.LocalDate.PlusDays(2));
                Assert.Contains(@"SET ""LocalDate"" = @p0", Db.Sql);
                Assert.Contains(@"p0='2020-10-12'", Db.Parameters);
            }

            [Fact]
            public void Select_Year()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDate.Year == 2020);
                Assert.Contains(@"WHERE CAST(strftime('%Y', ""n"".""LocalDate"") AS INTEGER) = 2020", Db.Sql);
            }

            [Fact]
            public void Select_Month()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDate.Month == 10);
                Assert.Contains(@"WHERE CAST(strftime('%m', ""n"".""LocalDate"") AS INTEGER) = 10", Db.Sql);
            }

            [Fact]
            public void Select_Day()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDate.Day == 10);
                Assert.Contains(@"WHERE CAST(strftime('%d', ""n"".""LocalDate"") AS INTEGER) = 10", Db.Sql);
            }

            [Fact]
            public void Select_DayOfYear()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDate.DayOfYear == 284);
                Assert.Contains(@"WHERE CAST(strftime('%j', ""n"".""LocalDate"") AS INTEGER) = 284", Db.Sql);
            }

            [Fact]
            public void Select_DayOfWeek()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalDate.DayOfWeek == IsoDayOfWeek.Saturday);
                Assert.Contains("WHERE CASE WHEN CAST(strftime('%w', \"n\".\"LocalDate\") AS INTEGER) = 0 THEN 7 " +
                                "ELSE CAST(strftime('%w', \"n\".\"LocalDate\") AS INTEGER) END = 6", Condense(Db.Sql));
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
            public void Select_Equal()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalTime == new LocalTime(23, 42, 16, 321));
                Assert.Contains(@"WHERE ""n"".""LocalTime"" = '23:42:16.321'", Db.Sql);
            }

            [Fact]
            public void Select_GreaterThan()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalTime > new LocalTime(23, 42, 00));
                Assert.Contains(@"WHERE ""n"".""LocalTime"" > '23:42:00'", Db.Sql);
            }

            [Fact]
            public void Select_LessThan()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalTime < new LocalTime(23, 50, 00));
                Assert.Contains(@"WHERE ""n"".""LocalTime"" < '23:50:00'", Db.Sql);
            }

            [Fact]
            public void Update()
            {
                RunUpdate(x => x.LocalTime = x.LocalTime.PlusSeconds(10));
                Assert.Contains(@"SET ""LocalTime"" = @p0", Db.Sql);
                Assert.Contains(@"p0='23:42:26.321'", Db.Parameters);
            }

            [Fact]
            public void Select_Hour()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalTime.Hour == 23);
                Assert.Contains(@"WHERE CAST(strftime('%H', ""n"".""LocalTime"") AS INTEGER) = 23", Db.Sql);
            }

            [Fact]
            public void Select_Minute()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalTime.Minute == 42);
                Assert.Contains(@"WHERE CAST(strftime('%M', ""n"".""LocalTime"") AS INTEGER) = 42", Db.Sql);
            }

            [Fact]
            public void Select_Second()
            {
                _ = Db.NodaTimeTypes.Single(x => x.LocalTime.Second == 16);
                Assert.Contains(@"WHERE CAST(strftime('%S', ""n"".""LocalTime"") AS INTEGER) = 16", Db.Sql);
            }
        }

        public class LocalDateMethodQueryTests : QueryTests
        {
            [Fact]
            public void PlusYears()
            {
                var value = Db.NodaTimeTypes.Select(x => x.LocalDate.PlusYears(2)).Single();
                Assert.Equal(new LocalDate(2022, 10, 10), value);
                Assert.Contains(@"SELECT date(""n"".""LocalDate"", '+2 years')", Db.Sql);
            }

            [Fact]
            public void PlusMonths()
            {
                var value = Db.NodaTimeTypes.Select(x => x.LocalDate.PlusMonths(2)).Single();
                Assert.Equal(new LocalDate(2020, 12, 10), value);
                Assert.Contains(@"SELECT date(""n"".""LocalDate"", '+2 months')", Db.Sql);
            }

            [Fact]
            public void PlusWeeks()
            {
                var value = Db.NodaTimeTypes.Select(x => x.LocalDate.PlusWeeks(2)).Single();
                Assert.Equal(new LocalDate(2020, 10, 24), value);
                Assert.Contains(@"SELECT date(""n"".""LocalDate"", '+14 days')", Db.Sql);
            }

            [Fact]
            public void PlusDays()
            {
                var value = Db.NodaTimeTypes.Select(x => x.LocalDate.PlusDays(2)).Single();
                Assert.Equal(new LocalDate(2020, 10, 12), value);
                Assert.Contains(@"SELECT date(""n"".""LocalDate"", '+2 days')", Db.Sql);
            }
        }

        public class LocalTimeMethodQueryTests : QueryTests
        {
            [Fact]
            public void PlusHours()
            {
                var value = Db.NodaTimeTypes.Select(x => x.LocalTime.PlusHours(2)).Single();
                Assert.Equal(new LocalTime(01, 42, 16, 321), value);
                Assert.Contains(@"SELECT CAST(strftime('%H:%M:%f', ""n"".""LocalTime"", '+2 hours') AS TEXT)", Db.Sql);
            }

            [Fact]
            public void PlusMinutes()
            {
                var value = Db.NodaTimeTypes.Select(x => x.LocalTime.PlusMinutes(2)).Single();
                Assert.Equal(new LocalTime(23, 44, 16, 321), value);
                Assert.Contains(@"SELECT CAST(strftime('%H:%M:%f', ""n"".""LocalTime"", '+2 minutes') AS TEXT)", Db.Sql);
            }

            [Fact]
            public void PlusSeconds()
            {
                var value = Db.NodaTimeTypes.Select(x => x.LocalTime.PlusSeconds(2)).Single();
                Assert.Equal(new LocalTime(23, 42, 18, 321), value);
                Assert.Contains(@"SELECT CAST(strftime('%H:%M:%f', ""n"".""LocalTime"", '+2 seconds') AS TEXT)", Db.Sql);
            }

            [Fact]
            public void PlusMilliseconds()
            {
                var value = Db.NodaTimeTypes.Select(x => x.LocalTime.PlusMilliseconds(2)).Single();
                Assert.Equal(new LocalTime(23, 42, 16, 323), value);
                Assert.Contains(@"SELECT CAST(strftime('%H:%M:%f', ""n"".""LocalTime"", '+0.002 seconds') AS TEXT)", Db.Sql);
            }
        }

        protected class NodaTimeContext : DbContext
        {
            private readonly TestLoggerFactory _loggerFactory = new();

            public string Sql => _loggerFactory.Logger.Sql;

            public string Parameters => _loggerFactory.Logger.Parameters;

            public DbSet<NodaTimeTypes> NodaTimeTypes { get; set; } = null!;

            protected override void OnConfiguring(DbContextOptionsBuilder options)
            {
                var builder = new SqliteConnectionStringBuilder
                {
                    DataSource = "NodaTime." + Guid.NewGuid() + ".db",
                    Cache = SqliteCacheMode.Private,
                };

                options
                    .UseSqlite(builder.ConnectionString, x => x.UseNodaTime())
                    .UseLoggerFactory(_loggerFactory)
                    .EnableSensitiveDataLogging();
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<NodaTimeTypes>()
                    .HasData(new NodaTimeTypes
                    {
                        LocalDateTime = LocalDateTimeQueryTests.Value,
                        LocalDate = LocalDateQueryTests.Value,
                        LocalTime = LocalTimeQueryTests.Value,
                        Instant = InstantQueryTests.Value,
                        Id = 1,
                    });
            }
        }

        protected class NodaTimeTypes
        {
            public int Id { get; set; }
            public Instant Instant { get; set; }
            public LocalTime LocalTime { get; set; }
            public LocalDate LocalDate { get; set; }
            public LocalDateTime LocalDateTime { get; set; }

        }
    }
}
