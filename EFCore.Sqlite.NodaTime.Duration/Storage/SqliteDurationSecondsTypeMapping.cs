using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage;

/// <summary>
/// A type mapping for NodaTime's <see cref="NodaTime.Duration"/>
/// type that stores the value as the total seconds (floating point).
/// </summary>
internal class SqliteDurationSecondsTypeMapping : RelationalTypeMapping
{
    public static readonly SqliteDurationSecondsTypeMapping Default = new();

    private SqliteDurationSecondsTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters) {}

    private SqliteDurationSecondsTypeMapping()
        : base(new RelationalTypeMappingParameters(
            new CoreTypeMappingParameters(typeof(double), new DurationSecondsConverter()),
            storeType: "REAL",
            dbType: System.Data.DbType.Double)) {}

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters) =>
        new SqliteDurationSecondsTypeMapping(parameters);

    protected override string SqlLiteralFormatString => "{0}";

    public override string GenerateSqlLiteral(object? value)
    {
        return value == null ? "NULL" : GenerateNonNullSqlLiteral(value);
    }

    protected override string GenerateNonNullSqlLiteral(object value)
    {
        if (value is long l)
        {
            // Duration has arithmetic overloads that take long values
            return l.ToString(null, CultureInfo.InvariantCulture) + ".0";
        }

        var seconds = value switch
        {
            double d => d,
            NodaTime.Duration d => d.TotalSeconds,
            _ => throw new NotImplementedException($"Unexpected value type {value.GetType()}")
        };

        // See DoubleTypeMapping.GenerateNonNullSqlLiteral
        var literal = seconds.ToString("G17", CultureInfo.InvariantCulture);

        return !literal.Contains('E')
               && !literal.Contains('e')
               && !literal.Contains('.')
               && !double.IsNaN(seconds)
               && !double.IsInfinity(seconds)
            ? literal + ".0"
            : literal;
    }

    private class DurationSecondsConverter : ValueConverter<NodaTime.Duration, double>
    {
        public DurationSecondsConverter()
            : base(
                d => d.TotalSeconds,
                d => NodaTime.Duration.FromSeconds(d))
        {
        }
    }
}
