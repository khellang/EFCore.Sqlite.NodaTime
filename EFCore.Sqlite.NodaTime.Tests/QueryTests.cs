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

        public void Dispose()
        {
            Db.Database.EnsureDeleted();
            Db.Dispose();
        }

        public class InstantQueryTests : QueryTests
        {
            [Fact]
            public void GetCurrentInstant_from_Instance()
            {
                var _ = Db.NodaTimeTypes
                    .Where(t => t.Instant < SystemClock.Instance.GetCurrentInstant())
                    .ToArray();

                Assert.Contains(@"WHERE ""n"".""Instant"" < strftime('%s', 'now')", Db.Sql);
            }
        }

        public class LocalDateTimeQueryTests : QueryTests
        {
            [Fact]
            public void Select_year()
            {
                var value = Db.NodaTimeTypes.Single(x => x.LocalDateTime.Year == 2020);

                Assert.Equal(new LocalDateTime(2020, 10, 10, 23, 42, 16, 321), value.LocalDateTime);
                Assert.Contains("", Db.Sql);
            }
        }

        protected class NodaTimeContext : DbContext
        {
            private readonly TestLoggerFactory _loggerFactory = new();

            public string Sql => _loggerFactory.Logger.Sql;

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
                    .UseLoggerFactory(_loggerFactory);
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                var localDateTime = new LocalDateTime(2020, 10, 10, 23, 42, 16, 321);
                var zonedDateTime = localDateTime.InUtc();
                var instant = zonedDateTime.ToInstant();

                modelBuilder.Entity<NodaTimeTypes>()
                    .HasData(new NodaTimeTypes
                    {
                        Id = 1,
                        Instant = instant,
                        LocalDateTime = localDateTime,
                    });
            }
        }

        protected class NodaTimeTypes
        {
            public int Id { get; set; }
            public Instant Instant { get; set; }

            public LocalDateTime LocalDateTime { get; set; }
        }
    }
}
