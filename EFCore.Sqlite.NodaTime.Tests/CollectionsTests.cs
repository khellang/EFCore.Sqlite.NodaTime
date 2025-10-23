using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite;

[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
public abstract class CollectionsTests<T> : IDisposable where T : struct
{
    protected CollectionsTests()
    {
        Db = NodaTimeContext.Create();
        Db.Database.EnsureCreated();
    }

    private NodaTimeContext Db { get; }

    protected IQueryable<NodaTimeTypesCollectionType<T>> Query => Db.Set<NodaTimeTypesCollectionType<T>>().AsQueryable();

    protected Task VerifyUpdate(Action<NodaTimeTypesCollectionType<T>> mutator, [CallerFilePath] string sourceFile = "")
    {
        Recording.Start();
        var set = Db.Set<NodaTimeTypesCollectionType<T>>();
        mutator(set.Single());
        Db.SaveChanges();
        return Verifier.Verify(Recording.Stop(), sourceFile: sourceFile);
    }

    protected Task VerifyQuery(Expression<Func<NodaTimeTypesCollectionType<T>, bool>> predicate, [CallerFilePath] string sourceFile = "")
    {
        Recording.Start();
        _ = Query.Where(predicate).Select(x => x.Id).Single();
        return Verifier.Verify(Recording.Stop(), sourceFile: sourceFile);
    }

    protected static void VerifyEqual(NodaTimeTypesCollectionType<T> expected, NodaTimeTypesCollectionType<T> actual)
    {
        // Use JSON serialization to compare collections
        var expectedJson = JsonSerializer.Serialize(expected);
        var actualJson = JsonSerializer.Serialize(actual);
        Assert.Equal(expectedJson, actualJson);
    }

    public void Dispose() => Db.Dispose();
}
