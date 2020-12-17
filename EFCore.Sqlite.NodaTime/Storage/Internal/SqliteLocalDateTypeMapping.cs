using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Sqlite.Extensions;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal.Converters;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class SqliteLocalDateTypeMapping : RelationalTypeMapping
    {
        private static readonly ConstructorInfo _constructor =
            typeof(LocalDate).GetConstructor(new[] { typeof(int), typeof(int), typeof(int) })!;

        public SqliteLocalDateTypeMapping() : base(CreateParameters())
        {
        }

        protected SqliteLocalDateTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
        {
        }

        protected override string SqlLiteralFormatString => "'{0}'";

        public override RelationalTypeMapping Clone(string storeType, int? size)
            => new SqliteLocalDateTypeMapping(Parameters.WithStoreTypeAndSize(storeType, size));

        public override CoreTypeMapping Clone(ValueConverter converter)
            => new SqliteLocalDateTypeMapping(Parameters.WithComposedConverter(converter));

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
            => new SqliteLocalDateTypeMapping(parameters);

        public override Expression GenerateCodeLiteral(object value)
            => GenerateCodeLiteral((LocalDate)value);

        private static Expression GenerateCodeLiteral(LocalDate value)
            => _constructor.ConstantNew(value.Year, value.Month, value.Day);

        private static RelationalTypeMappingParameters CreateParameters()
            => new(new CoreTypeMappingParameters(typeof(LocalDate), SqliteLocalDateValueConverter.Instance), "TEXT");
    }
}
