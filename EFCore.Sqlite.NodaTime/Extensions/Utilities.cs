using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Sqlite.Extensions
{
    internal static class Utilities
    {
        internal static SqlExpression Date(this ISqlExpressionFactory factory, Type returnType, SqlExpression timestring, IEnumerable<SqlExpression>? modifiers = null)
            => factory.DateFunction("date", GetArguments(timestring, modifiers), returnType);

        internal static SqlExpression Strftime(this ISqlExpressionFactory factory, Type returnType, string format, SqlExpression timestring, IEnumerable<SqlExpression>? modifiers = null)
        {
            var result = factory.DateFunction("strftime", GetArguments(timestring, modifiers).Insert(0, factory.Constant(format)), timestring.Type);
            if (timestring.Type != returnType)
            {
                result = factory.Convert(result, returnType);
            }
            return result;
        }

        internal static SqlExpression JulianDay(this ISqlExpressionFactory factory, SqlExpression argument, Type returnType)
            => factory.DateFunction("JULIANDAY", ImmutableList<SqlExpression>.Empty.Add(argument), returnType);

        private static SqlExpression DateFunction(this ISqlExpressionFactory factory, string name, IImmutableList<SqlExpression> arguments, Type returnType)
            => factory.Function(name, arguments, nullable: true, arguments.Select(_ => true), returnType);

        private static ImmutableList<SqlExpression> GetArguments(SqlExpression timestring, IEnumerable<SqlExpression>? modifiers)
        {
            modifiers ??= [];

            if (ShouldUnwrap(timestring, out var function, out var timestringIndex))
            {
                timestring = function.Arguments![timestringIndex];
                modifiers = function.Arguments.Skip(timestringIndex + 1).Concat(modifiers);
            }

            var builder = ImmutableList.CreateBuilder<SqlExpression>();

            builder.Add(timestring);

            builder.AddRange(modifiers);

            return builder.ToImmutable();
        }

        private static bool ShouldUnwrap(SqlExpression timestring, [NotNullWhen(true)] out SqlFunctionExpression? function, out int timestringIndex)
        {
            if (timestring is SqlUnaryExpression unary)
            {
                timestring = unary.Operand;
            }

            if (timestring is SqlFunctionExpression fn && fn.Arguments is not null)
            {
                switch (fn.Name)
                {
                    case "strftime" when fn.Arguments.Count > 2:
                        timestringIndex = 1;
                        function = fn;
                        return true;
                    case "date" or "time" or "datetime" when fn.Arguments.Count > 1:
                        timestringIndex = 0;
                        function = fn;
                        return true;
                }
            }

            timestringIndex = -1;
            function = default;
            return false;
        }
    }
}
