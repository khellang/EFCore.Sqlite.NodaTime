using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators.Internal;

internal class SqliteNodaTimeMethodCallTranslatorPlugin(ISqlExpressionFactory sqlExpressionFactory) : IMethodCallTranslatorPlugin
{
    public IEnumerable<IMethodCallTranslator> Translators { get; } = [
        new SqliteNodaTimeDateDiffFunctionsTranslator(sqlExpressionFactory),
        new SqliteNodaTimeMethodCallTranslator(sqlExpressionFactory)
    ];
}
