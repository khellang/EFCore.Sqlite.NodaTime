using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public class SqliteNodaTimeServiceCollectionExtensionsTests
    {
        [Fact]
        public static void CallingExtensionMethodWithNullThrows()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                SqliteNodaTimeServiceCollectionExtensions.AddEntityFrameworkSqliteNodaTime(null!));
            Assert.Equal("services", ex.ParamName);
        }
    }
}
