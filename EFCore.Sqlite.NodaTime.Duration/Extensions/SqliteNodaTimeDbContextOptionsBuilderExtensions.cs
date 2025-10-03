using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Sqlite.Infrastructure;
using Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore;

public static class SqliteNodaTimeDbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Adds the services required to use NodaTime <see cref="NodaTime.Duration"/> with the Sqlite provider.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Do NOT use this mapping when storing the exact value is critical.
    /// </para>
    /// <para>
    /// <see cref="NodaTime.Duration"/> will be stored as a <see cref="double"/> representing the total number of seconds.
    /// As a result of floating point precision, the value may lose precision when saved to the database.
    /// This enables arithmetic operations on durations in the SQLite database at the expense of exact round-tripping.
    /// </para>
    /// <para>
    /// If you need exact round-tripping of <see cref="NodaTime.Duration"/>,
    /// consider using the provided round-trip converter instead:
    /// <code>
    /// using Microsoft.EntityFrameworkCore.Sqlite;
    ///
    /// protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    /// {
    ///     configurationBuilder.Properties&lt;Duration&gt;()
    ///         .HaveConversion&lt;DurationPatternConverter&gt;();
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    public static SqliteDbContextOptionsBuilder UseNodaTimeDuration(this SqliteDbContextOptionsBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var coreBuilder = ((IRelationalDbContextOptionsBuilderInfrastructure)builder).OptionsBuilder;

        var extension = coreBuilder.Options.FindExtension<SqliteNodaTimeDurationExtension>()
                        ?? new SqliteNodaTimeDurationExtension();

        ((IDbContextOptionsBuilderInfrastructure)coreBuilder).AddOrUpdateExtension(extension);

        return builder;
    }
}
