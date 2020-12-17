using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Sqlite.Extensions;
using Microsoft.EntityFrameworkCore.Storage;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class SqliteLocalTimeTypeMapping : SqliteTypeMapping<LocalTime>
    {
        private static readonly ConstructorInfo _constructor =
            typeof(LocalTime).GetConstructor(new[] { typeof(int), typeof(int), typeof(int), typeof(int) })!;

        public SqliteLocalTimeTypeMapping() : base(LocalTimePattern.ExtendedIso)
        {
        }

        protected SqliteLocalTimeTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
        {
        }

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
            => new SqliteLocalTimeTypeMapping(parameters);

        public override Expression GenerateCodeLiteral(object value)
            => GenerateCodeLiteral((LocalTime)value);

        private static Expression GenerateCodeLiteral(LocalTime value)
            => _constructor.ConstantNew(value.Hour, value.Minute, value.Second, value.Millisecond);
    }
}
