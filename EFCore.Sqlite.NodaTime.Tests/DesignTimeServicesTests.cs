using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Sqlite.Design.Internal;
using Microsoft.EntityFrameworkCore.Sqlite.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite;

public class DesignTimeServicesTests
{
    [Fact]
    public void ConfigureDesignTimeServices_works()
    {
        var serviceCollection = new ServiceCollection();
        new SqliteNodaTimeDesignTimeServices().ConfigureDesignTimeServices(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        Assert.IsType<SqliteNodaTimeTypeMappingSourcePlugin>(serviceProvider.GetService<IRelationalTypeMappingSourcePlugin>());
        var plugin = Assert.IsType<SqliteNodaTimeCodeGeneratorPlugin>(serviceProvider.GetService<IProviderCodeGeneratorPlugin>());
        Assert.NotNull(plugin.GenerateProviderOptions().MethodInfo);
    }
}