using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators.Internal;
using Microsoft.EntityFrameworkCore.Sqlite.Query.Internal;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class SqliteNodaTimeServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkSqliteNodaTime(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            new EntityFrameworkRelationalServicesBuilder(services)
                .TryAdd<IRelationalTypeMappingSourcePlugin, SqliteNodaTimeTypeMappingSourcePlugin>()
                .TryAdd<IMethodCallTranslatorPlugin, SqliteNodaTimeMethodCallTranslatorPlugin>()
                .TryAdd<IMemberTranslatorPlugin, SqliteNodaTimeMemberTranslatorPlugin>()
                .TryAdd<IEvaluatableExpressionFilterPlugin, SqliteNodaTimeEvaluatableExpressionFilterPlugin>();

            return services;
        }
    }
}
