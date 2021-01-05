using Microsoft.EntityFrameworkCore.Storage;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class SqliteLocalDateTimeTypeMapping : SqliteTypeMapping<LocalDateTime>
    {
        private static readonly LocalDateTimePattern _pattern =
            LocalDateTimePattern.CreateWithInvariantCulture("uuuu'-'MM'-'dd' 'HH':'mm':'ss'.'FFFFFFFFF");

        public SqliteLocalDateTimeTypeMapping() : base(_pattern)
        {
        }

        protected SqliteLocalDateTimeTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
        {
        }

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
            => new SqliteLocalDateTimeTypeMapping(parameters);
    }
}
