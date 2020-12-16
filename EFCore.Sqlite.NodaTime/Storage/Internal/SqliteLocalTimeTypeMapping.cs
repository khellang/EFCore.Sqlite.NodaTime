using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Sqlite.Extensions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class SqliteLocalTimeTypeMapping : RelationalTypeMapping
    {
        private static readonly ConstructorInfo _constructor =
            typeof(LocalTime).GetConstructor(new[] { typeof(int), typeof(int), typeof(int) })!;

        public SqliteLocalTimeTypeMapping() : base(CreateParameters())
        {
        }

        protected SqliteLocalTimeTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
        {
        }

        protected override string SqlLiteralFormatString => "time('{0}')";

        public override RelationalTypeMapping Clone(string storeType, int? size)
            => new SqliteLocalTimeTypeMapping(Parameters.WithStoreTypeAndSize(storeType, size));

        public override CoreTypeMapping Clone(ValueConverter converter)
            => new SqliteLocalTimeTypeMapping(Parameters.WithComposedConverter(converter));

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
            => new SqliteLocalTimeTypeMapping(parameters);

        public override Expression GenerateCodeLiteral(object value)
            => GenerateCodeLiteral((LocalTime)value);

        private static Expression GenerateCodeLiteral(LocalTime value)
            => _constructor.ConstantNew(value.Hour, value.Minute, value.Second);

        private static RelationalTypeMappingParameters CreateParameters()
            => new(new CoreTypeMappingParameters(typeof(LocalTime), new LocalTimeValueConverter()), "TEXT");

        private class LocalTimeValueConverter : ValueConverter<LocalTime, string>
        {
            public LocalTimeValueConverter() : base(
                d => LocalTimePattern.ExtendedIso.Format(d),
                t => LocalTimePattern.ExtendedIso.Parse(t).GetValueOrThrow())
            {
            }
        }
    }
}
