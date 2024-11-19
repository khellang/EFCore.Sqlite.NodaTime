using System.Threading.Tasks;
using NodaTime;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Sqlite;

public class LocalDateTimeDateDiffQueryTests : MethodQueryTests<LocalDateTime>
{
    public LocalDateTimeDateDiffQueryTests() : base(x => x.LocalDateTime)
    {
    }

    [Fact]
    public Task DateDiffYear() => VerifyMethod(x => EF.Functions
        .DateDiffYear(x, LocalDateTimeQueryTests.Value.PlusYears(42)));

    [Fact]
    public Task DateDiffMonth() => VerifyMethod(x => EF.Functions
        .DateDiffMonth(x, LocalDateTimeQueryTests.Value.PlusMonths(42)));

    [Fact]
    public Task DateDiffWeek() => VerifyMethod(x => EF.Functions
        .DateDiffWeek(x, LocalDateTimeQueryTests.Value.PlusWeeks(42)));

    [Fact]
    public Task DateDiffHour() => VerifyMethod(x => EF.Functions
        .DateDiffHour(x, LocalDateTimeQueryTests.Value.PlusHours(42)));

    [Fact]
    public Task DateDiffMinute() => VerifyMethod(x => EF.Functions
        .DateDiffMinute(x, LocalDateTimeQueryTests.Value.PlusMinutes(42)));

    [Fact]
    public Task DateDiffSecond() => VerifyMethod(x => EF.Functions
        .DateDiffSecond(x, LocalDateTimeQueryTests.Value.PlusSeconds(42)));

    [Fact]
    public Task DateDiffMillisecond() => VerifyMethod(x => EF.Functions
        .DateDiffMillisecond(x, LocalDateTimeQueryTests.Value.PlusMilliseconds(42)));
}