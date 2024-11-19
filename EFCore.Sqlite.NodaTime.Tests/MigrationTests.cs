using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.Extensions.DependencyInjection;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

[assembly: DesignTimeServicesReference(
    "Microsoft.EntityFrameworkCore.Sqlite.Design.Internal.SqliteNodaTimeDesignTimeServices, EntityFrameworkCore.Sqlite.NodaTime",
    "Microsoft.EntityFrameworkCore.Sqlite")]

namespace Microsoft.EntityFrameworkCore.Sqlite;

public class MigrationTests
{
    public MigrationTests(ITestOutputHelper output)
    {
        Output = output;
    }

    private ITestOutputHelper Output { get; }

    [Fact]
    public Task Migration_Generates_Correct_MigrationCode()
    {
        return Verify(Output, "MigrationCode", migration => migration.MigrationCode);
    }

    [Fact]
    public Task Migration_Generates_Correct_SnapshotCode()
    {
        return Verify(Output, "SnapshotCode", migration => migration.SnapshotCode);
    }

    private static Task Verify(ITestOutputHelper output, string migrationName, Func<ScaffoldedMigration, string> selector)
    {
        return Verifier.Verify(selector(ScaffoldMigration(output, migrationName)));
    }

    private static ScaffoldedMigration ScaffoldMigration(ITestOutputHelper output, string migrationName)
    {
        var reporter = new OperationReporter(
            new OperationReportHandler(
                m => output.WriteLine($"  error: {m}"),
                m => output.WriteLine($"   warn: {m}"),
                m => output.WriteLine($"   info: {m}"),
                m => output.WriteLine($"verbose: {m}")));

        var assembly = System.Reflection.Assembly.GetExecutingAssembly();

        using var context = NodaTimeContext.Create();

        return new DesignTimeServicesBuilder(assembly, assembly, reporter, Array.Empty<string>())
            .Build(context)
            .GetRequiredService<IMigrationsScaffolder>()
            .ScaffoldMigration(migrationName, "Microsoft.EntityFrameworkCore.Sqlite");
    }
}