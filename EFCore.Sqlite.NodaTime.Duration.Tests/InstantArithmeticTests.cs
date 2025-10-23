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
    public async Task Plus()
    {
        var result = await VerifySelect(x => x.Instant.Plus(x.Duration));
        Assert.Equal(result, InstantValue + DurationQueryTests.Value);
    }

    [Fact]
    public async Task PlusNegative()
    {
        var result = await VerifySelect(x => x.Instant.Plus(-x.Duration));
        Assert.Equal(result, InstantValue - DurationQueryTests.Value);
    }

    [Fact]
    public async Task Minus()
    {
        var result = await VerifySelect(x => x.Instant.Minus(x.Duration));
        Assert.Equal(result, InstantValue - DurationQueryTests.Value);
    }

    [Fact]
    public async Task MinusNegative()
    {
        var result = await VerifySelect(x => x.Instant.Minus(-x.Duration));
        Assert.Equal(result, InstantValue + DurationQueryTests.Value);
    }

    [Fact]
    public async Task Chain()
    {
        var result = await VerifySelect(x => x.Instant
            .Plus(x.Duration)
            .Minus(x.Duration)
            .Plus(x.Duration)
            .Minus(x.Duration));
        Assert.Equal(result, InstantValue);
    }

    [Fact]
    public Task ChainGetCurrentInstantPlus() => VerifyQuery(x =>
        SystemClock.Instance.GetCurrentInstant().Plus(x.Duration).Minus(x.Duration)
        == SystemClock.Instance.GetCurrentInstant());

    [Fact]
    public Task ChainGetCurrentInstantMinus() => VerifyQuery(x =>
        SystemClock.Instance.GetCurrentInstant().Minus(x.Duration).Plus(x.Duration)
        == SystemClock.Instance.GetCurrentInstant());

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
