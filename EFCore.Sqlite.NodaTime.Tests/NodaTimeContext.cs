using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VerifyTests;

namespace Microsoft.EntityFrameworkCore.Sqlite;

public sealed class NodaTimeContext : DbContext
{
    public NodaTimeContext(DbContextOptions<NodaTimeContext> options) : base(options)
    {
        Connection = RelationalOptionsExtension.Extract(options).Connection;
    }

    private DbConnection? Connection { get; }

    public DbSet<NodaTimeTypes> NodaTimeTypes { get; set; } = null!;

    public static NodaTimeContext Create()
    {
        var connection = new SqliteConnection("Filename=:memory:");

        connection.Open();

        var builder = new DbContextOptionsBuilder<NodaTimeContext>()
            .UseSqlite(connection, x => x.UseNodaTime())
            .EnableSensitiveDataLogging();

        builder.EnableRecording();

        return new NodaTimeContext(builder.Options);
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

        var model = modelBuilder.Model;

        model.RemoveAnnotation(CoreAnnotationNames.ProductVersion);
    }

    public override void Dispose()
    {
        base.Dispose();
        Connection?.Dispose();
    }
}