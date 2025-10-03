using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;

internal class SqliteTypeMapping<T> : RelationalTypeMapping
{
    protected SqliteTypeMapping(IPattern<T> pattern) : this(CreateParameters(pattern))
    {
    }

    protected SqliteTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
    {
    }

    protected override string SqlLiteralFormatString => "'{0}'";

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
        => new SqliteTypeMapping<T>(parameters);

    private static RelationalTypeMappingParameters CreateParameters(IPattern<T> pattern)
        => new(new CoreTypeMappingParameters(typeof(T), new NodaTimePatternConverter(pattern)), "TEXT");

    private class NodaTimePatternConverter(IPattern<T> pattern) : ValueConverter<T, string>(
        x => pattern.Format(x),
        x => pattern.Parse(x).GetValueOrThrow());
}
