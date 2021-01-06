using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VerifyTests.EntityFramework;
using VerifyXunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    [UsesVerify]
    public abstract class QueryTests<T> : IDisposable
    {
        protected QueryTests(Expression<Func<NodaTimeTypes, T>> selector)
        {
            Selector = selector;
            Db = NodaTimeContext.Create();
            Db.Database.EnsureCreated();
        }

        private Expression<Func<NodaTimeTypes, T>> Selector { get; }

        protected NodaTimeContext Db { get; }

        protected IQueryable<T> Query => Db.NodaTimeTypes.Select(Selector);

        protected Task RunUpdate(Action<NodaTimeTypes> mutator, [CallerFilePath] string sourceFile = "")
        {
            SqlRecording.StartRecording();
            mutator(Db.NodaTimeTypes.Single());
            Db.SaveChanges();
            return Verifier.Verify(SqlRecording.FinishRecording(), sourceFile: sourceFile);
        }

        public void Dispose()
        {
            Db.Database.EnsureDeleted();
            Db.Dispose();
        }
    }
}
