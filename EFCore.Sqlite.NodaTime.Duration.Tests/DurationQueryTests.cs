using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using Xunit;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Microsoft.EntityFrameworkCore.Sqlite;

public class DurationQueryTests : QueryTests<Duration>
{
    public DurationQueryTests()
        : base(x => x.Duration)
    {
    }

    public static readonly Duration Value = Duration.FromSeconds(130641.516); // 36.28931 hours

    [Fact]
    public void Roundtrip() => Assert.Equal(Value, Query.Single());

    [Fact]
    public Task TotalDays() => VerifyQuery(x => x.TotalDays < 5);

    [Fact]
    public Task TotalHours() => VerifyQuery(x => x.TotalHours > 12.1);

    [Fact]
    public Task TotalMinutes() => VerifyQuery(x => x.TotalMinutes > 90.6);

    [Fact]
    public Task TotalSeconds() => VerifyQuery(x => x.TotalSeconds == 130641.516);

    [Fact]
    public Task TotalMilliseconds() => VerifyQuery(x => x.TotalMilliseconds < 1_000_000_000);

    [Fact]
    public Task SelectTotalDays() => VerifySelect(x => (int)x.TotalDays);

    [Fact]
    public Task SelectTotalSeconds() => VerifySelect(x => x.TotalSeconds);

    [Fact]
    public Task Arithmetic() => VerifySelect(x => new
    {
        Multiply = 2 * x,
        Divide = x / 2.0,
        Negate = -x,

        Add = x + Duration.FromHours(x.TotalHours),
        Subtract = x - Duration.FromMinutes(x.TotalHours)
    });

    [Fact]
    public Task FromDays() => VerifyQuery(x => x == Duration.FromDays(x.TotalDays));

    [Fact]
    public Task FromHours() => VerifyQuery(x => x == Duration.FromHours(x.TotalHours));

    [Fact]
    public Task FromMinutes() => VerifyQuery(x => x != Duration.FromMinutes((long)x.TotalMinutes));

    [Fact]
    public Task FromSeconds() => VerifyQuery(x => x == Duration.FromSeconds(x.TotalSeconds));

    [Fact]
    public Task FromMilliseconds() => VerifyQuery(x => x == Duration.FromMilliseconds(x.TotalMilliseconds));
}
