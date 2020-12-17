using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal.Converters;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class SqliteInstantTypeMapping : RelationalTypeMapping
    {
        private static readonly MethodInfo _fromUnixTimeTicks =
            typeof(Instant).GetRuntimeMethod(nameof(Instant.FromUnixTimeTicks), new[] { typeof(long) })!;

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
            => new(new CoreTypeMappingParameters(typeof(Instant), SqliteInstantValueConverter.Instance), "TEXT");
    }
}
