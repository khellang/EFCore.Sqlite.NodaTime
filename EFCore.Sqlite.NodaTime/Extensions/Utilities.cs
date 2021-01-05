using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Sqlite.Extensions
{
    internal static class Utilities
    {
        internal static NewExpression ConstantNew(this ConstructorInfo constructor, params object?[] parameters)
            => Expression.New(constructor, parameters.Select(Expression.Constant).Cast<Expression>().ToArray());

        internal static SqlExpression Date(this ISqlExpressionFactory factory, Type returnType, SqlExpression timestring, IEnumerable<SqlExpression>? modifiers = null)
            => factory.DateFunction("date", GetArguments(timestring, modifiers), returnType);

        internal static SqlExpression Time(this ISqlExpressionFactory factory, Type returnType, SqlExpression timestring, IEnumerable<SqlExpression>? modifiers = null)
            => factory.DateFunction("time", GetArguments(timestring, modifiers), returnType);

        internal static SqlExpression DateTime(this ISqlExpressionFactory factory, Type returnType, SqlExpression timestring, IEnumerable<SqlExpression>? modifiers = null)
            => factory.DateFunction("datetime", GetArguments(timestring, modifiers), returnType);

        internal static SqlExpression Strftime(this ISqlExpressionFactory factory, Type returnType, string format, SqlExpression timestring, IEnumerable<SqlExpression>? modifiers = null)
            => factory.Convert(factory.DateFunction("strftime", GetArguments(timestring, modifiers).Insert(0, factory.Constant(format)), typeof(string)), returnType);

        private static SqlExpression DateFunction(this ISqlExpressionFactory factory, string name, IImmutableList<SqlExpression> arguments, Type returnType)
            => factory.Function(name, arguments, nullable: true, arguments.Select(_ => true), returnType);

        private static IImmutableList<SqlExpression> GetArguments(SqlExpression timestring, IEnumerable<SqlExpression>? modifiers)
        {
            modifiers ??= Enumerable.Empty<SqlExpression>();

            if (ShouldUnwrap(timestring, out var function))
            {
                timestring = function.Arguments[0];
                modifiers = function.Arguments.Skip(1).Concat(modifiers);
            }

            var builder = ImmutableList.CreateBuilder<SqlExpression>();

            builder.Add(timestring);

            builder.AddRange(modifiers);

            return builder.ToImmutable();
        }

        private static bool ShouldUnwrap(SqlExpression timestring, [NotNullWhen(true)] out SqlFunctionExpression? function)
        {
            if (timestring is SqlFunctionExpression fn)
            {
                if (fn.Name == "date" || fn.Name == "time" || fn.Name == "datetime")
                {
                    if (fn.Arguments.Count > 1)
                    {
                        function = fn;
                        return true;
                    }
                }
            }

            function = default;
            return false;
        }
    }
}
