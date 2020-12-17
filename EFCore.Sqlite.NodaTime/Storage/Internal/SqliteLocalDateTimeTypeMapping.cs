using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Sqlite.Extensions;
using Microsoft.EntityFrameworkCore.Storage;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class SqliteLocalDateTimeTypeMapping : SqliteTypeMapping<LocalDateTime>
    {
        private static readonly LocalDateTimePattern _pattern =
            LocalDateTimePattern.CreateWithInvariantCulture("uuuu'-'MM'-'dd' 'HH':'mm':'ss'.'FFFFFFFFF");

        private static readonly ConstructorInfo _constructor =
            typeof(LocalDateTime).GetConstructor(new[]
            {
                typeof(int), // year
                typeof(int), // month
                typeof(int), // day
                typeof(int), // hour
                typeof(int), // minute
                typeof(int), // second
                typeof(int), // millisecond
            })!;

        public SqliteLocalDateTimeTypeMapping() : base(_pattern)
        {
        }

        protected SqliteLocalDateTimeTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
        {
        }

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
            => new SqliteLocalDateTimeTypeMapping(parameters);

        public override Expression GenerateCodeLiteral(object value)
            => GenerateCodeLiteral((LocalDateTime)value);

        private static Expression GenerateCodeLiteral(LocalDateTime value)
            => _constructor.ConstantNew(
                value.Year,
                value.Month,
                value.Day,
                value.Hour,
                value.Minute,
                value.Second,
                value.Millisecond);
    }
}
