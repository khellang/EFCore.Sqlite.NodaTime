using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VerifyTests;
using VerifyXunit;

namespace Microsoft.EntityFrameworkCore.Sqlite;

[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
public abstract class QueryTests<T> : IDisposable
{
    protected QueryTests(Expression<Func<NodaTimeTypes, T>> selector)
    {
        Selector = selector;
        Db = NodaTimeContext.Create();
        Db.Database.EnsureCreated();
    }

    private Expression<Func<NodaTimeTypes, T>> Selector { get; }

    private NodaTimeContext Db { get; }

    protected IQueryable<T> Query => Db.NodaTimeTypes.Select(Selector);

    protected Task VerifyQuery(Expression<Func<T, bool>> predicate, [CallerFilePath] string sourceFile = "")
    {
        Recording.Start();
        _ = Query.Single(predicate);
        return Verifier.Verify(Recording.Stop(), sourceFile: sourceFile);
    }

    protected async Task<TOut> VerifySelect<TOut>(Expression<Func<T, TOut>> selector, [CallerFilePath] string sourceFile = "")
    {
        Recording.Start();
        var result = Query.Select(selector).Single();
        await Verifier.Verify(Recording.Stop(), sourceFile: sourceFile);
        return result;
    }

    public void Dispose() => Db.Dispose();
}
