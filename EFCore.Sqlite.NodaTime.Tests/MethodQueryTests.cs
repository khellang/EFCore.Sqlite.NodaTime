using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VerifyTests;
using VerifyXunit;

namespace Microsoft.EntityFrameworkCore.Sqlite;

public abstract class MethodQueryTests<T> : QueryTests<T> where T : struct
{
    protected MethodQueryTests(Expression<Func<NodaTimeTypes, T>> selector) : base(selector)
    {
    }

    protected Task VerifyMethod<TResult>(Expression<Func<T, TResult>> selector, [CallerFilePath] string sourceFile = "")
    {
        Recording.Start();
        var value = Query.Select(selector).Single();
        // ReSharper disable once ExplicitCallerInfoArgument
        return Verifier.Verify(value, sourceFile: sourceFile);
    }
}
