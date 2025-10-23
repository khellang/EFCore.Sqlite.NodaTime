using System.Collections.Immutable;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NodaTime;
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

        modelBuilder.Entity<NodaTimeTypesCollectionType<LocalDateTime>>()
            .ToTable("LocalDateTimeCollections")
            .HasData(NewCollection(LocalDateTimeQueryTests.CollectionValues));
        modelBuilder.Entity<NodaTimeTypesCollectionType<LocalDate>>()
            .ToTable("LocalDateCollections")
            .HasData(NewCollection(LocalDateQueryTests.CollectionValues));
        modelBuilder.Entity<NodaTimeTypesCollectionType<LocalTime>>()
            .ToTable("LocalTimeCollections")
            .HasData(NewCollection(LocalTimeQueryTests.CollectionValues));
        modelBuilder.Entity<NodaTimeTypesCollectionType<Instant>>()
            .ToTable("InstantCollections")
            .HasData(NewCollection(InstantQueryTests.CollectionValues));

        var model = modelBuilder.Model;

        model.RemoveAnnotation(CoreAnnotationNames.ProductVersion);
    }

    public override void Dispose()
    {
        base.Dispose();
        Connection?.Dispose();
    }

    public static NodaTimeTypesCollectionType<T> NewCollection<T>(T[] items) where T : struct
    {
        var nullable = items.SelectMany(x => new T?[] { x, null }).ToArray();
        return new NodaTimeTypesCollectionType<T>
        {
            Id = 1,
            Array = items.ToArray(),
            List = items.ToList(),
            IList = items.Append(items[0]).ToList(),
            IReadOnlyList = items.ToImmutableList(),

            ArrayNullable = nullable.ToArray(),
            ListNullable = nullable.ToList(),
            IListNullable = nullable.Append(items[0]).ToList(),
            IReadOnlyListNullable = nullable.ToImmutableList(),
        };
    }
}
