using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators.Internal;

public class SqliteNodaTimeMemberTranslatorPlugin(ISqlExpressionFactory sqlExpressionFactory) : IMemberTranslatorPlugin
{
    public virtual IEnumerable<IMemberTranslator> Translators { get; } =
    [
        new SqliteNodaTimeMemberTranslator(sqlExpressionFactory)
    ];
}