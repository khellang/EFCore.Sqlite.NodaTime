using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite;

public class SqliteNodaTimeTypeMappingSourcePluginTests
{
    [Fact]
    public void CallingFindMappingWithNullClrType_ReturnsNull()
    {
        var plugin = new SqliteNodaTimeTypeMappingSourcePlugin();
        Assert.Null(plugin.FindMapping(new EntityFrameworkCore.Storage.RelationalTypeMappingInfo()));
    }
}
