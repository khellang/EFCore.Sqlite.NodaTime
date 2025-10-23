using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite;

public class InstantQueryTests : QueryTests<Instant>
{
    public static readonly Instant Value = LocalDateTimeQueryTests.Value.InUtc().ToInstant();

    public static readonly Instant[] CollectionValues =
        LocalDateTimeQueryTests.CollectionValues.Select(v => v.InUtc().ToInstant()).ToArray();

    public InstantQueryTests() : base(x => x.Instant)
    {
    }

    [Fact]
    public void Roundtrip() => Assert.Equal(Value, Query.Single());

    [Fact]
    public Task GetCurrentInstant_From_Instance() => VerifyQuery(x => x < SystemClock.Instance.GetCurrentInstant());

    [Fact]
    public Task EnumerableContains() => VerifyQuery(i => CollectionValues.Contains(i));

    public class Collections : CollectionsTests<Instant>
    {
        [Fact]
        public void Roundtrip() => VerifyEqual(NodaTimeContext.NewCollection(CollectionValues), Query.Single());

        [Fact]
        public Task Intersects() => VerifyQuery(x => x.List.Intersect(CollectionValues.Take(2)).Any());
    }
}
