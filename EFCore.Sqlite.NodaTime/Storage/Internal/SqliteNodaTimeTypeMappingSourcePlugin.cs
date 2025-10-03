using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;

internal class SqliteNodaTimeTypeMappingSourcePlugin : IRelationalTypeMappingSourcePlugin
{
    private readonly Dictionary<Type, RelationalTypeMapping> _clrTypeMappings = new(capacity: 4)
    {
        { typeof(LocalDateTime), SqliteLocalDateTimeTypeMapping.Default },
        { typeof(LocalDate), SqliteLocalDateTypeMapping.Default },
        { typeof(LocalTime), SqliteLocalTimeTypeMapping.Default },
        { typeof(Instant), SqliteInstantTypeMapping.Default }
    };

    public RelationalTypeMapping? FindMapping(in RelationalTypeMappingInfo mappingInfo) =>
        mappingInfo.ClrType is null ? null : _clrTypeMappings.GetValueOrDefault(mappingInfo.ClrType);
}
