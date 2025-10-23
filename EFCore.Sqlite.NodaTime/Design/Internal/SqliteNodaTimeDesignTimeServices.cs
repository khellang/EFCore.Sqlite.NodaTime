using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Sqlite.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Sqlite.Design.Internal;

// ReSharper disable once UnusedType.Global
public class SqliteNodaTimeDesignTimeServices : IDesignTimeServices
{
    public virtual void ConfigureDesignTimeServices(IServiceCollection services) =>
        services
            .AddSingleton<IRelationalTypeMappingSourcePlugin, SqliteNodaTimeTypeMappingSourcePlugin>()
            .AddSingleton<IProviderCodeGeneratorPlugin, SqliteNodaTimeCodeGeneratorPlugin>();
}
