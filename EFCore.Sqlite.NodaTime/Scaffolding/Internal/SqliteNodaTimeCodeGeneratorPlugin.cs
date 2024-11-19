using System.Reflection;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace Microsoft.EntityFrameworkCore.Sqlite.Scaffolding.Internal;

public class SqliteNodaTimeCodeGeneratorPlugin : ProviderCodeGeneratorPlugin
{
    public override MethodCallCodeFragment GenerateProviderOptions()
    {
        var method = typeof(SqliteNodaTimeDbContextOptionsBuilderExtensions)
            .GetMethod(nameof(SqliteNodaTimeDbContextOptionsBuilderExtensions.UseNodaTime),
                BindingFlags.Public | BindingFlags.Static);

        return new MethodCallCodeFragment(method!);
    }
}