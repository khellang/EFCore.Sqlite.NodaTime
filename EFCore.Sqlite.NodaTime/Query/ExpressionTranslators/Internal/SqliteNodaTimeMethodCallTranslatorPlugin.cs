using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators.Internal
{
    public class SqliteNodaTimeMethodCallTranslatorPlugin(ISqlExpressionFactory sqlExpressionFactory) : IMethodCallTranslatorPlugin
    {
        public virtual IEnumerable<IMethodCallTranslator> Translators { get; } =
        [
            new SqliteNodaTimeDateDiffFunctionsTranslator(sqlExpressionFactory),
            new SqliteNodaTimeMethodCallTranslator(sqlExpressionFactory)
        ];
    }
}
