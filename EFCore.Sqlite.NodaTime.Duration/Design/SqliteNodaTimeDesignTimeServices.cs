using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Sqlite.Scaffolding;
using Microsoft.EntityFrameworkCore.Sqlite.Storage;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Sqlite.Design;

// ReSharper disable once UnusedType.Global
public class SqliteNodaTimeDurationDesignTimeServices : IDesignTimeServices
{
    public virtual void ConfigureDesignTimeServices(IServiceCollection services) =>
        services
            .AddSingleton<IRelationalTypeMappingSourcePlugin, SqliteNodaTimeDurationTypeMappingSourcePlugin>()
            .AddSingleton<IProviderCodeGeneratorPlugin, SqliteNodaTimeDurationCodeGeneratorPlugin>();
}
