using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Sqlite.Extensions;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal.Converters;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class SqliteLocalDateTimeTypeMapping : RelationalTypeMapping
    {
        private static readonly ConstructorInfo _constructorWithSeconds =
            typeof(LocalDateTime).GetConstructor(new[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int) })!;

        private static readonly ConstructorInfo _constructorWithMinutes =
            typeof(LocalDateTime).GetConstructor(new[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int) })!;

        public SqliteLocalDateTimeTypeMapping() : base(CreateParameters())
        {
        }

        protected SqliteLocalDateTimeTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
        {
        }

        protected override string SqlLiteralFormatString => "'{0}'";

        public override RelationalTypeMapping Clone(string storeType, int? size)
            => new SqliteLocalDateTimeTypeMapping(Parameters.WithStoreTypeAndSize(storeType, size));

        public override CoreTypeMapping Clone(ValueConverter converter)
            => new SqliteLocalDateTimeTypeMapping(Parameters.WithComposedConverter(converter));

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
            => new SqliteLocalDateTimeTypeMapping(parameters);

        public override Expression GenerateCodeLiteral(object value)
            => GenerateCodeLiteral((LocalDateTime)value);

        private static Expression GenerateCodeLiteral(LocalDateTime value)
        {
            if (value.Second == 0 && value.NanosecondOfSecond == 0)
            {
                return _constructorWithMinutes.ConstantNew(value.Year, value.Month, value.Day, value.Hour, value.Minute);
            }

            return _constructorWithSeconds.ConstantNew(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second);
        }

        private static RelationalTypeMappingParameters CreateParameters()
            => new(new CoreTypeMappingParameters(typeof(LocalDateTime), LocalDateTimeValueConverter.Instance), "TEXT");
    }
}
