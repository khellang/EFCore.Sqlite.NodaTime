using System;
using System.Linq;
using Microsoft.Data.Sqlite;
using NodaTime;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public class QueryTests : IDisposable
    {
        public QueryTests()
        {
            Db = new NodaTimeContext();
            Db.Database.EnsureCreated();
        }

        private NodaTimeContext Db { get; }

        [Fact]
        public void GetCurrentInstant_from_Instance()
        {
            var _ = Db.NodaTimeTypes
                .Where(t => t.Instant < SystemClock.Instance.GetCurrentInstant())
                .ToArray();

            Assert.Contains(@"WHERE ""n"".""Instant"" < strftime('%s', 'now')", Db.Sql);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        private class NodaTimeContext : DbContext
        {
            private readonly TestLoggerFactory _loggerFactory = new();

            public string Sql => _loggerFactory.Logger.Sql;

            public DbSet<NodaTimeTypes> NodaTimeTypes { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder options)
            {
                var builder = new SqliteConnectionStringBuilder
                {
                    DataSource = "NodaTime" + nameof(QueryTests) + ".db",
                    Cache = SqliteCacheMode.Shared,
                };

                options
                    .UseSqlite(builder.ConnectionString, x => x.UseNodaTime())
                    .UseLoggerFactory(_loggerFactory);
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                var localDateTime = new LocalDateTime(2018, 4, 20, 10, 31, 33, 666);
                var zonedDateTime = localDateTime.InUtc();
                var instant = zonedDateTime.ToInstant();

                modelBuilder.Entity<NodaTimeTypes>()
                    .HasData(new NodaTimeTypes { Id = 1, Instant = instant });
            }
        }

        private class NodaTimeTypes
        {
            public int Id { get; set; }
            public Instant Instant { get; set; }
        }
    }
}
