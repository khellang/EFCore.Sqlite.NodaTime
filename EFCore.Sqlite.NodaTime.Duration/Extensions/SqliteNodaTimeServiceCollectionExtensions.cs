using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators;
using Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators.Internal;
using Microsoft.EntityFrameworkCore.Sqlite.Query.Internal;
using Microsoft.EntityFrameworkCore.Sqlite.Storage;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class SqliteNodaTimeServiceCollectionExtensions
{
    public static IServiceCollection AddEntityFrameworkSqliteNodaTimeDuration(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        new EntityFrameworkRelationalServicesBuilder(services)
            .TryAdd<IRelationalTypeMappingSourcePlugin, SqliteNodaTimeDurationTypeMappingSourcePlugin>()
            .TryAdd<IMethodCallTranslatorPlugin, SqliteNodaTimeDurationPlugin>()
            .TryAdd<IMemberTranslatorPlugin, SqliteNodaTimeDurationPlugin>();

        return services;
    }
}
