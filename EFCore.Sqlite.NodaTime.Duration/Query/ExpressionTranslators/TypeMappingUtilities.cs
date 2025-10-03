using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.ExpressionTranslators;

internal static class TypeMappingUtilities
{
    internal static RelationalTypeMapping GetRelationalTypeMapping(this ITypeMappingSource typeMappingSource, Type clrType)
    {
        var result = typeMappingSource.FindMapping(clrType) as RelationalTypeMapping;
        Debug.Assert(result != null, $"No relational mapping for {clrType} could be found.");
        return result;
    }

    /// <summary>
    /// Applies the given type mapping to the expression.
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="ISqlExpressionFactory.ApplyTypeMapping"/>,
    /// this method will always apply the given mapping even if the expression already has a type mapping.
    /// </remarks>
    public static SqlExpression ChangeExpressionType(
        this ISqlExpressionFactory sqlExpressionFactory,
        SqlExpression expression,
        RelationalTypeMapping mapping)
    {
        if (expression.TypeMapping == mapping)
        {
            // Already has the correct mapping
            return expression;
        }

        if (expression.TypeMapping?.DbType != mapping.DbType)
        {
            // Types do not match, so we need to convert
            return sqlExpressionFactory.Convert(expression, mapping.ClrType, mapping);
        }

        // Types match, so we just need to apply the mapping, for some expression types we can do this directly
        return expression switch
        {
            ColumnExpression column => column.ApplyTypeMapping(mapping),
            SqlConstantExpression constant => constant.ApplyTypeMapping(mapping),
            SqlFunctionExpression function => function.ApplyTypeMapping(mapping),
            SqlParameterExpression parameter => parameter.ApplyTypeMapping(mapping),
            SqlBinaryExpression binary => new SqlBinaryExpression(binary.OperatorType, binary.Left, binary.Right, binary.Type, mapping),

            // For other expression types we need to convert
            _ => sqlExpressionFactory.Convert(expression, mapping.ClrType, mapping)
        };
    }
}
