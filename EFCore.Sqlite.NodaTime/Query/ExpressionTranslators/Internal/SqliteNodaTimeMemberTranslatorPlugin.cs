using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators.Internal
{
    public class SqliteNodaTimeMemberTranslatorPlugin : IMemberTranslatorPlugin
    {
        public SqliteNodaTimeMemberTranslatorPlugin(ISqlExpressionFactory sqlExpressionFactory)
        {
            Translators = new IMemberTranslator[]
            {
                new SqliteNodaTimeMemberTranslator(sqlExpressionFactory),
            };
        }

        public virtual IEnumerable<IMemberTranslator> Translators { get; }
    }
}
