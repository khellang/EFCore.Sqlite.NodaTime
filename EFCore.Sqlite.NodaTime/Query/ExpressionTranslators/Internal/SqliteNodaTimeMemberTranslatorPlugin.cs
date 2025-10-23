using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators.Internal;

internal class SqliteNodaTimeMemberTranslatorPlugin(ISqlExpressionFactory sqlExpressionFactory) : IMemberTranslatorPlugin
{
    public IEnumerable<IMemberTranslator> Translators { get; } = [
        new SqliteNodaTimeMemberTranslator(sqlExpressionFactory)
    ];
}
