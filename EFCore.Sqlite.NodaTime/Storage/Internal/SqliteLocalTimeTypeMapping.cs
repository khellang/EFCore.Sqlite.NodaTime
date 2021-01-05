using Microsoft.EntityFrameworkCore.Storage;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class SqliteLocalTimeTypeMapping : SqliteTypeMapping<LocalTime>
    {
        public SqliteLocalTimeTypeMapping() : base(LocalTimePattern.ExtendedIso)
        {
        }

        protected SqliteLocalTimeTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
        {
        }

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
            => new SqliteLocalTimeTypeMapping(parameters);
    }
}
