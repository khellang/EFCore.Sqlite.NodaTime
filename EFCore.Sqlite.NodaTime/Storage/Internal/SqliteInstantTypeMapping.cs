using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class SqliteInstantTypeMapping : RelationalTypeMapping
    {
        private static readonly InstantPattern _pattern =
            InstantPattern.CreateWithInvariantCulture("uuuu'-'MM'-'dd' 'HH':'mm':'ss'.'FFFFFFFFF");

        private static readonly MethodInfo _fromUnixTimeTicks =
            typeof(Instant).GetRuntimeMethod(nameof(Instant.FromUnixTimeTicks), new[] { typeof(long) })!;

        public SqliteInstantTypeMapping() : this(CreateParameters())
        {
        }

        protected SqliteInstantTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
        {
        }

        protected override string SqlLiteralFormatString => "'{0}'";
        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
            => new SqliteInstantTypeMapping(parameters);

        public override Expression GenerateCodeLiteral(object value)
            => GenerateCodeLiteral((Instant)value);

        private static Expression GenerateCodeLiteral(Instant value)
            => Expression.Call(_fromUnixTimeTicks, Expression.Constant(value.ToUnixTimeTicks()));

        private static RelationalTypeMappingParameters CreateParameters()
            => new(new CoreTypeMappingParameters(typeof(Instant), SqliteValueConverter.Create(_pattern)), "TEXT");
    }
}
