using Microsoft.EntityFrameworkCore.Storage;
using NodaTime;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public class SqliteInstantTypeMapping : SqliteTypeMapping<Instant>
    {
        private static readonly InstantPattern _pattern =
            InstantPattern.CreateWithInvariantCulture("uuuu'-'MM'-'dd' 'HH':'mm':'ss'.'FFFFFFFFF");

        public SqliteInstantTypeMapping() : base(_pattern)
        {
        }

        protected SqliteInstantTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
        {
        }

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
            => new SqliteInstantTypeMapping(parameters);
    }
}
