using System.Threading.Tasks;
using NodaTime;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite;

public class InstantArithmeticTests : QueryTests<NodaTimeTypes>
{
    public InstantArithmeticTests()
        : base(x => x)
    {
    }

    public static readonly LocalDateTime LocalValue = new(2020, 10, 10, 23, 42, 16, 321);

    public static readonly Instant InstantValue = LocalValue.InUtc().ToInstant();

    [Fact]
    public Task DurationBetween() => VerifySelect(x =>
        EF.Functions.DurationBetween(x.Instant, SystemClock.Instance.GetCurrentInstant()));

    [Fact]
    public Task DurationBetweenTotalSeconds() => VerifySelect(x =>
        EF.Functions.DurationBetween(x.Instant, x.Instant).TotalSeconds);

    [Fact]
    public Task DurationBetweenTotalMinutes() => VerifySelect(x =>
        EF.Functions.DurationBetween(x.Instant, SystemClock.Instance.GetCurrentInstant()).TotalMinutes);

    [Fact]
    public Task DurationBetweenTotalNanoseconds() => VerifyQuery(x =>
        (int)EF.Functions.DurationBetween(x.Instant, SystemClock.Instance.GetCurrentInstant()).TotalNanoseconds > 0);

    [Fact]
    public Task DurationBetweenTotalDays() => VerifyQuery(x =>
        EF.Functions.DurationBetween(SystemClock.Instance.GetCurrentInstant(), x.Instant).TotalDays < 0);

    [Fact]
    public Task DurationFromDays() => VerifyQuery(x => Duration.FromDays(x.LocalDateTime.Day) == Duration.FromDays(10));
}
