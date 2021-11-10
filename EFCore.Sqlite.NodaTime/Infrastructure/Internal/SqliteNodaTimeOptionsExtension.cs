using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal
{
    public class SqliteNodaTimeOptionsExtension : IDbContextOptionsExtension
    {
        private DbContextOptionsExtensionInfo? _info;

        public DbContextOptionsExtensionInfo Info => _info ??= new ExtensionInfo(this);

        public void ApplyServices(IServiceCollection services) => services.AddEntityFrameworkSqliteNodaTime();

        public void Validate(IDbContextOptions options)
        {
            var provider = options.FindExtension<CoreOptionsExtension>()?.InternalServiceProvider;
            if (provider is null)
            {
                return;
            }

            using var scope = provider.CreateScope();

            var services = scope.ServiceProvider
                .GetServices<IRelationalTypeMappingSourcePlugin>()
                .OfType<SqliteNodaTimeTypeMappingSourcePlugin>();

            if (services.Any())
            {
                throw new InvalidOperationException($"{nameof(SqliteNodaTimeDbContextOptionsBuilderExtensions.UseNodaTime)} requires {nameof(SqliteNodaTimeServiceCollectionExtensions.AddEntityFrameworkSqliteNodaTime)} to be called on the internal service provider used.");
            }
        }

        private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
        {
            public ExtensionInfo(IDbContextOptionsExtension extension)
                : base(extension)
            {
            }

            public override bool IsDatabaseProvider => false;

            public override int GetServiceProviderHashCode() => 0;

            public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
                => debugInfo["Sqlite:" + nameof(SqliteNodaTimeDbContextOptionsBuilderExtensions.UseNodaTime)] = "1";

            public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => true;

            public override string LogFragment => "using NodaTime ";
        }
    }
}
