using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;

internal class SqliteNodaTimeTypeMappingSourcePlugin : IRelationalTypeMappingSourcePlugin
{
    static SqliteNodaTimeTypeMappingSourcePlugin()
    {
        _clrTypeMappings = new Dictionary<Type, RelationalTypeMapping>();
        Add(SqliteInstantTypeMapping.Default);
        Add(SqliteLocalDateTypeMapping.Default);
        Add(SqliteLocalDateTimeTypeMapping.Default);
        Add(SqliteLocalTimeTypeMapping.Default);
        _clrTypeMappings.TrimExcess();

        return;

        static void Add<T>(SqliteNodaTimeTypeMapping<T> typeMapping)
            where T : struct
        {
            // Add the element type itself
            _clrTypeMappings.Add(typeof(T), typeMapping);

            // Add collection mappings

            var list = SqliteNodaTimeTypeMapping<T>.CreateCollectionMapping<List<T>>(typeMapping);
            var array = SqliteNodaTimeTypeMapping<T>.CreateCollectionMapping<T[]>(typeMapping);
            var listNullable = SqliteNodaTimeTypeMapping<T>.CreateCollectionNullableMapping<List<T?>>(typeMapping);
            var arrayNullable = SqliteNodaTimeTypeMapping<T>.CreateCollectionNullableMapping<T?[]>(typeMapping);

            _clrTypeMappings.Add(typeof(IEnumerable<T>), list);
            _clrTypeMappings.Add(typeof(T[]), array);
            _clrTypeMappings.Add(typeof(List<T>), list);
            _clrTypeMappings.Add(typeof(IList<T>), list);
            _clrTypeMappings.Add(typeof(IReadOnlyList<T>), list);

            _clrTypeMappings.Add(typeof(IEnumerable<T?>), listNullable);
            _clrTypeMappings.Add(typeof(T?[]), arrayNullable);
            _clrTypeMappings.Add(typeof(List<T?>), listNullable);
            _clrTypeMappings.Add(typeof(IList<T?>), listNullable);
            _clrTypeMappings.Add(typeof(IReadOnlyList<T?>), listNullable);
        }
    }

    private static readonly Dictionary<Type, RelationalTypeMapping> _clrTypeMappings;

    public RelationalTypeMapping? FindMapping(in RelationalTypeMappingInfo mappingInfo)
    {
        return mappingInfo.ClrType is null ? null : _clrTypeMappings.GetValueOrDefault(mappingInfo.ClrType);
    }
}
