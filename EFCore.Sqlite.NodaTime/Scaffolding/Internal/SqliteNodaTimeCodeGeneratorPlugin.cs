using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace Microsoft.EntityFrameworkCore.Sqlite.Scaffolding.Internal
{
    public class SqliteNodaTimeCodeGeneratorPlugin : ProviderCodeGeneratorPlugin
    {
        public override MethodCallCodeFragment GenerateProviderOptions() =>
            new(nameof(SqliteNodaTimeDbContextOptionsBuilderExtensions.UseNodaTime));
    }
}
