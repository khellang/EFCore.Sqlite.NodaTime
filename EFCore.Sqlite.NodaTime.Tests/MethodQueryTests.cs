using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VerifyTests.EntityFramework;
using VerifyXunit;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public abstract class MethodQueryTests<T> : QueryTests<T>
    {
        protected MethodQueryTests(Expression<Func<NodaTimeTypes, T>> selector) : base(selector)
        {
        }

        protected Task Verify(Expression<Func<T, T>> selector, [CallerFilePath] string sourceFile = "")
        {
            SqlRecording.StartRecording();
            var value = Query.Select(selector).Single();
            return Verifier.Verify(value, sourceFile: sourceFile);
        }
    }
}
