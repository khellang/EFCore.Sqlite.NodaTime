using Microsoft.EntityFrameworkCore.Sqlite.Extensions;
using Microsoft.EntityFrameworkCore.Storage;
using NodaTime.Text;

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal
{
    public abstract class SqliteTypeMapping<T> : RelationalTypeMapping
    {
        protected SqliteTypeMapping(IPattern<T> pattern) : this(CreateParameters(pattern))
        {
        }

        protected SqliteTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
        {
        }

        protected override string SqlLiteralFormatString => "'{0}'";

        private static RelationalTypeMappingParameters CreateParameters(IPattern<T> pattern)
            => new(new CoreTypeMappingParameters(typeof(T), pattern.AsValueConverter()), "TEXT");
    }
}
