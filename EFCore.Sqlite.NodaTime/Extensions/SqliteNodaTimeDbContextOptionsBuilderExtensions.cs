using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore;

public static class SqliteNodaTimeDbContextOptionsBuilderExtensions
{
    public static SqliteDbContextOptionsBuilder UseNodaTime(this SqliteDbContextOptionsBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var coreBuilder = ((IRelationalDbContextOptionsBuilderInfrastructure)builder).OptionsBuilder;

        var extension = coreBuilder.Options.FindExtension<SqliteNodaTimeOptionsExtension>()
                        ?? new SqliteNodaTimeOptionsExtension();

        ((IDbContextOptionsBuilderInfrastructure)coreBuilder).AddOrUpdateExtension(extension);

        return builder;
    }
}