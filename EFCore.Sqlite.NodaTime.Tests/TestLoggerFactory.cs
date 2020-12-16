using System;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public class TestLoggerFactory : ILoggerFactory
    {
        public readonly TestLogger Logger = new();

        public void AddProvider(ILoggerProvider provider) => throw new NotSupportedException();

        public ILogger CreateLogger(string categoryName) => Logger;

        public void Dispose()
        {
        }
    }
}
