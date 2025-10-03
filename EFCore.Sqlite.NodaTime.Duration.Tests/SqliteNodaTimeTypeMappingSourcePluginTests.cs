using Microsoft.EntityFrameworkCore.Sqlite.Storage;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite;

public class SqliteNodaTimeTypeMappingSourcePluginTests
{
    [Fact]
    public void CallingFindMappingWithNullClrType_ReturnsNull()
    {
        var plugin = new SqliteNodaTimeDurationTypeMappingSourcePlugin();
        Assert.Null(plugin.FindMapping(new EntityFrameworkCore.Storage.RelationalTypeMappingInfo()));
    }
}
