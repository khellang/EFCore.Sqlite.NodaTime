using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage;
using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class SqliteNodaTimeTypeMappingSourcePlugin : IRelationalTypeMappingSourcePlugin
    {
        private readonly Dictionary<Type, RelationalTypeMapping> _clrTypeMappings = new Dictionary<Type, RelationalTypeMapping>
        {
            { typeof(LocalDateTime), new SqliteLocalDateTimeTypeMapping() },
            { typeof(LocalDate), new SqliteLocalDateTypeMapping() },
            { typeof(LocalTime), new SqliteLocalTimeTypeMapping() },
            { typeof(Instant), new SqliteInstantTypeMapping() },
        };

        public RelationalTypeMapping? FindMapping(in RelationalTypeMappingInfo mappingInfo)
        {
            if (mappingInfo.ClrType is null)
            {
                return null;
            }

            if (_clrTypeMappings.TryGetValue(mappingInfo.ClrType, out var mapping))
            {
                return mapping;
            }

            return null;
        }
    }
}