using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VerifyTests;
using VerifyXunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
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

        protected Task VerifyUpdate(Action<NodaTimeTypes> mutator, [CallerFilePath] string sourceFile = "")
        {
            Recording.Start();
            mutator(Db.NodaTimeTypes.Single());
            Db.SaveChanges();
            return Verifier.Verify(Recording.Stop(), sourceFile: sourceFile);
        }

        protected Task VerifyQuery(Expression<Func<T, bool>> predicate, [CallerFilePath] string sourceFile = "")
        {
            Recording.Start();
            _ = Query.Single(predicate);
            return Verifier.Verify(Recording.Stop(), sourceFile: sourceFile);
        }

        public void Dispose() => Db.Dispose();
    }
}
