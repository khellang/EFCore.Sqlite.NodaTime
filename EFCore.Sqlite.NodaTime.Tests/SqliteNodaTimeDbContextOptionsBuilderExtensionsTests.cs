using System;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public class SqliteNodaTimeDbContextOptionsBuilderExtensionsTests
    {
        [Fact]
        public static void CallingExtensionMethodWithNullThrows()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                SqliteNodaTimeDbContextOptionsBuilderExtensions.UseNodaTime(null!));
            Assert.Equal("builder", ex.ParamName);
        }
    }
}
