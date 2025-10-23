using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;

[SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
internal class SqliteNodaTimeTypeMapping<T> : RelationalTypeMapping where T : struct
{
    protected SqliteNodaTimeTypeMapping(IPattern<T> pattern) : this(CreateParameters(pattern))
    {
    }

    protected SqliteNodaTimeTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
    {
    }

    protected override string SqlLiteralFormatString => "'{0}'";

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
        => new SqliteNodaTimeTypeMapping<T>(parameters);

    private static RelationalTypeMappingParameters CreateParameters(IPattern<T> pattern)
        => new(new CoreTypeMappingParameters(typeof(T), new NodaTimePatternConverter(pattern)), "TEXT");

    private class NodaTimePatternConverter(IPattern<T> pattern) : ValueConverter<T, string>(
        x => pattern.Format(x),
        x => pattern.Parse(x).GetValueOrThrow());


    /// <summary>
    /// Creates a mapping for a collection of NodaTime structs (i.e. JSON array of strings).
    /// </summary>
    public static RelationalTypeMapping CreateCollectionMapping<TCollection>(
        SqliteNodaTimeTypeMapping<T> elementTypeMapping)
        where TCollection : IEnumerable<T>
    {
        var elementJsonReaderWriter = new SqliteNodaTimeJsonStringReaderWriter<T>();
        var valueComparer = ValueComparer.CreateDefault<T>(false);

        var comparer = new ListOfValueTypesComparer<TCollection, T>(valueComparer);
        var jsonValueReaderWriter = new JsonCollectionOfStructsReaderWriter<TCollection, T>(elementJsonReaderWriter);

        return CreateCollectionMappingInternal<T>(elementTypeMapping, jsonValueReaderWriter, comparer);
    }

    /// <summary>
    /// Creates a mapping for a collection of nullable NodaTime structs (i.e. JSON array of strings or nulls).
    /// </summary>
    public static RelationalTypeMapping CreateCollectionNullableMapping<TCollection>(SqliteNodaTimeTypeMapping<T> elementTypeMapping)
        where TCollection : IEnumerable<T?>
    {
        var elementJsonReaderWriter = new SqliteNodaTimeJsonStringReaderWriter<T>();
        var valueComparer = new NullableValueComparer<T>(ValueComparer.CreateDefault<T>(false));

        var comparer = new ListOfNullableValueTypesComparer<TCollection, T>(valueComparer);
        var jsonValueReaderWriter = new JsonCollectionOfNullableStructsReaderWriter<TCollection, T>(elementJsonReaderWriter);

        return CreateCollectionMappingInternal<T?>(elementTypeMapping, jsonValueReaderWriter, comparer);
    }

    /// <summary>
    /// Creates a mapping for a collection of nullable NodaTime structs (i.e. JSON array of strings or nulls).
    /// </summary>
    private static RelationalTypeMapping CreateCollectionMappingInternal<TElement>(
        SqliteNodaTimeTypeMapping<T> elementTypeMapping,
        JsonValueReaderWriter jsonValueReaderWriter,
        ValueComparer comparer)
    {
        var result = SqliteStringTypeMapping.Default.Clone(
            elementMapping: elementTypeMapping,
            jsonValueReaderWriter: jsonValueReaderWriter,
            comparer: comparer,
            keyComparer: comparer,
            converter: new CollectionToJsonStringConverter<TElement>(jsonValueReaderWriter),
            providerValueComparer: ValueComparer.CreateDefault<string>(false));
        return result;
    }
}
