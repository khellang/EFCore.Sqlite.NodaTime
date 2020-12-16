using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.Internal
{
    public class SqliteNodaTimeEvaluatableExpressionFilterPlugin : IEvaluatableExpressionFilterPlugin
    {
        private static readonly MethodInfo _getCurrentInstantMethod =
            typeof(SystemClock).GetRuntimeMethod(nameof(SystemClock.GetCurrentInstant), Array.Empty<Type>())!;

        private static readonly MemberInfo _systemClockInstanceMember =
            typeof(SystemClock).GetMember(nameof(SystemClock.Instance)).FirstOrDefault()!;

        public bool IsEvaluatableExpression(Expression expression)
        {
            return expression switch
            {
                MethodCallExpression exp when exp.Method == _getCurrentInstantMethod => false,
                MemberExpression exp when exp.Member == _systemClockInstanceMember => false,
                _ => true
            };
        }
    }
}
