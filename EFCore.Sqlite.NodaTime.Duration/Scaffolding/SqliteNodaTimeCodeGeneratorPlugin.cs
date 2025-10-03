using System.Reflection;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace Microsoft.EntityFrameworkCore.Sqlite.Scaffolding;

internal class SqliteNodaTimeDurationCodeGeneratorPlugin : ProviderCodeGeneratorPlugin
{
    public override MethodCallCodeFragment GenerateProviderOptions()
    {
        var method = typeof(SqliteNodaTimeDbContextOptionsBuilderExtensions)
            .GetMethod(nameof(SqliteNodaTimeDbContextOptionsBuilderExtensions.UseNodaTimeDuration),
                BindingFlags.Public | BindingFlags.Static);

        return new MethodCallCodeFragment(method!);
    }
}
