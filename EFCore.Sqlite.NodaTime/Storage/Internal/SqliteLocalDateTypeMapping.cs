using Microsoft.EntityFrameworkCore.Storage;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class SqliteLocalDateTypeMapping : SqliteTypeMapping<LocalDate>
    {
        public SqliteLocalDateTypeMapping() : base(LocalDatePattern.Iso)
        {
        }

        protected SqliteLocalDateTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
        {
        }

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
            => new SqliteLocalDateTypeMapping(parameters);
    }
}
