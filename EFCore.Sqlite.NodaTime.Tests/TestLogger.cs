using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public class TestLogger : ILogger
    {
        public string Sql { get; private set; } = null!;

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) => NoOpDisposable.Instance;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (eventId == RelationalEventId.CommandExecuting)
            {
                if (state is IReadOnlyList<KeyValuePair<string, object>> structure)
                {
                    var commandText = structure.FirstOrDefault(i => i.Key == "commandText");
                    if (commandText is { Value: string sql })
                    {
                        Sql = sql;
                    }
                }
            }
        }

        private class NoOpDisposable : IDisposable
        {
            public static readonly IDisposable Instance = new NoOpDisposable();

            private NoOpDisposable()
            {
            }

            public void Dispose()
            {
            }
        }
    }
}
