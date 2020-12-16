using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class SqliteInstantTypeMapping : RelationalTypeMapping
    {
        private static readonly MethodInfo _fromUnixTimeSeconds =
            typeof(Instant).GetRuntimeMethod(nameof(Instant.FromUnixTimeSeconds), new[] { typeof(long) })!;

        public SqliteInstantTypeMapping() : base(CreateParameters())
        {
        }

        protected SqliteInstantTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
        {
        }

        public override RelationalTypeMapping Clone(string storeType, int? size)
            => new SqliteInstantTypeMapping(Parameters.WithStoreTypeAndSize(storeType, size));

        public override CoreTypeMapping Clone(ValueConverter converter)
            => new SqliteInstantTypeMapping(Parameters.WithComposedConverter(converter));

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
            => new SqliteInstantTypeMapping(parameters);

        protected override string GenerateNonNullSqlLiteral(object value)
            => ((long)value).ToString(CultureInfo.InvariantCulture);

        public override Expression GenerateCodeLiteral(object value)
            => GenerateCodeLiteral((Instant)value);

        private static Expression GenerateCodeLiteral(Instant value)
            => Expression.Call(_fromUnixTimeSeconds, Expression.Constant(value.ToUnixTimeSeconds()));

        private static RelationalTypeMappingParameters CreateParameters()
            => new(new CoreTypeMappingParameters(typeof(Instant), new InstantValueConverter()), "INTEGER");

        private class InstantValueConverter : ValueConverter<Instant, long>
        {
            public InstantValueConverter() : base(
                i => i.ToUnixTimeSeconds(),
                t => Instant.FromUnixTimeSeconds(t))
            {
            }
        }
    }
}
