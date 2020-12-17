using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class SqliteInstantTypeMapping : RelationalTypeMapping
    {
        private static readonly MethodInfo _fromUnixTimeTicks =
            typeof(Instant).GetRuntimeMethod(nameof(Instant.FromUnixTimeTicks), new[] { typeof(long) })!;

        private static readonly InstantPattern _pattern =
            InstantPattern.CreateWithInvariantCulture("uuuu'-'MM'-'dd' 'HH':'mm':'ss'.'FFFFFFFFF");

        public SqliteInstantTypeMapping() : base(CreateParameters())
        {
        }

        protected SqliteInstantTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
        {
        }

        protected override string SqlLiteralFormatString => "'{0}'";

        public override RelationalTypeMapping Clone(string storeType, int? size)
            => new SqliteInstantTypeMapping(Parameters.WithStoreTypeAndSize(storeType, size));

        public override CoreTypeMapping Clone(ValueConverter converter)
            => new SqliteInstantTypeMapping(Parameters.WithComposedConverter(converter));

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
            => new SqliteInstantTypeMapping(parameters);

        public override Expression GenerateCodeLiteral(object value)
            => GenerateCodeLiteral((Instant)value);

        private static Expression GenerateCodeLiteral(Instant value)
            => Expression.Call(_fromUnixTimeTicks, Expression.Constant(value.ToUnixTimeTicks()));

        private static RelationalTypeMappingParameters CreateParameters()
            => new(new CoreTypeMappingParameters(typeof(Instant), new InstantValueConverter()), "TEXT");

        private class InstantValueConverter : ValueConverter<Instant, string>
        {
            public InstantValueConverter() : base(
                i => _pattern.Format(i),
                t => _pattern.Parse(t).GetValueOrThrow())
            {
            }
        }
    }
}
