using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Sqlite.Extensions;
using Microsoft.EntityFrameworkCore.Storage;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class SqliteLocalDateTypeMapping : SqliteTypeMapping<LocalDate>
    {
        private static readonly ConstructorInfo _constructor =
            typeof(LocalDate).GetConstructor(new[] { typeof(int), typeof(int), typeof(int) })!;

        public SqliteLocalDateTypeMapping() : base(LocalDatePattern.Iso)
        {
        }

        protected SqliteLocalDateTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
        {
        }

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
            => new SqliteLocalDateTypeMapping(parameters);

        public override Expression GenerateCodeLiteral(object value)
            => GenerateCodeLiteral((LocalDate)value);

        private static Expression GenerateCodeLiteral(LocalDate value)
            => _constructor.ConstantNew(value.Year, value.Month, value.Day);
    }
}
