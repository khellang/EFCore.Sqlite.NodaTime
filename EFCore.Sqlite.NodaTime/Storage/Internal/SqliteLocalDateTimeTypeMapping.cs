using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Sqlite.Extensions;
using Microsoft.EntityFrameworkCore.Storage;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class SqliteLocalDateTimeTypeMapping : RelationalTypeMapping
    {
        private static readonly LocalDateTimePattern _pattern =
            LocalDateTimePattern.CreateWithInvariantCulture("uuuu'-'MM'-'dd' 'HH':'mm':'ss'.'FFFFFFFFF");

        private static readonly ConstructorInfo _constructorWithSeconds =
            typeof(LocalDateTime).GetConstructor(new[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int) })!;

        private static readonly ConstructorInfo _constructorWithMinutes =
            typeof(LocalDateTime).GetConstructor(new[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int) })!;

        public SqliteLocalDateTimeTypeMapping() : this(CreateParameters())
        {
        }

        protected SqliteLocalDateTimeTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
        {
        }

        protected override string SqlLiteralFormatString => "'{0}'";

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
            => new(new CoreTypeMappingParameters(typeof(LocalDateTime), SqliteValueConverter.Create(_pattern)), "TEXT");
    }
}
