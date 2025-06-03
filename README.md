# EFCore.Sqlite.NodaTime

![Build](https://github.com/khellang/EFCore.Sqlite.NodaTime/workflows/Build/badge.svg)

Adds support for [NodaTime](https://github.com/nodatime/nodatime) types when using [SQLite](https://sqlite.org/) with [Entity Framework Core](https://github.com/dotnet/efcore).

## Installation

[![NuGet](https://img.shields.io/nuget/v/EntityFrameworkCore.Sqlite.NodaTime)](https://www.nuget.org/packages/EntityFrameworkCore.Sqlite.NodaTime)

Install the latest package from [NuGet](https://www.nuget.org/packages/EntityFrameworkCore.Sqlite.NodaTime).

## Getting Started

If you're using Entity Framework Core without Dependency Injection, you can call `UseNodaTime` inside the `OnConfiguring` method in your `DbContext` class:

```csharp
public class MyDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("<connection-string>", x => x.UseNodaTime());
    }
}
```

Otherwise, you should call `UseNodaTime` when adding the `DbContext` to your service collection:

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<MyDbContext>(options =>
            options.UseSqlite("<connection-string>", x => x.UseNodaTime()));
    }
}
```

And that's it. You can now use NodaTime types in your entities and perform server-side queries on them! :sparkles:

## Supported Types

The following NodaTime types are currently supported:

| NodaTime Type | SQLite Type | SQLite Format |
|---------------|-------------|---------------|
| `Instant` | `TEXT` | `YYYY-MM-DD HH:MM:SS.SSS` |
| `LocalDateTime` | `TEXT` | `YYYY-MM-DD HH:MM:SS.SSS` |
| `LocalDate` | `TEXT` | `YYYY-MM-DD` |
| `LocalTime` | `TEXT` | `HH:MM:SS.SSS` |

# Supported Properties

| NodaTime Property | Generated SQL | Notes |
|-------------------|--------------|-------|
| `Year` | `strftime('%Y', <column>)` | The value is cast to `INTEGER` for comparison. |
| `Month` | `strftime('%m', <column>)` | The value is cast to `INTEGER` for comparison. |
| `Day` | `strftime('%d', <column>)` | The value is cast to `INTEGER` for comparison. |
| `Hour` | `strftime('%H', <column>)` | The value is cast to `INTEGER` for comparison. |
| `Minute` | `strftime('%M', <column>)` | The value is cast to `INTEGER` for comparison. |
| `Second` | `strftime('%S', <column>)` | The value is cast to `INTEGER` for comparison. |
| `DayOfYear` | `strftime('%j', <column>)` | The value is cast to `INTEGER` for comparison.|
| `DayOfWeek` | `strftime('%w', <column>)` | The value is cast to `INTEGER` for comparison. As NodaTime's `IsoDayOfWeek` enum doesn't match SQLite's day of week, additional SQL is emitted to convert `Sunday` to the correct value. |
| `Date` | `date(<column>)` | |
| `TimeOfDay` | `strftime('%H:%M:%f', <column>)` | In order to support fractional seconds to fully roundtrip `LocalTime`, a custom format string is used instead of using `time(<column>)`. |

## Supported Methods

| NodaTime Method | Generated SQL |
|-----------------|---------------|
| `SystemClock.Instance.GetCurrentInstant()` | `strftime('%Y-%m-%d %H:%M:%f', 'now')` |
| `LocalDate.PlusYears` | `date(<column>, '+n years')` |
| `LocalDate.PlusMonths` | `date(<column>, '+n months')` |
| `LocalDate.PlusWeeks` | `date(<column>, '+n*7 days')` |
| `LocalDate.PlusDays` | `date(<column>, '+n days')` |
| `LocalDate.ToDateOnly` | `date(<column>)` |
| `LocalDate.ToDateTimeUnspecified` | `date(<column>)` |
| `LocalTime.PlusHours` | `strftime('%H:%M:%f', <column>, '+n hours')` |
| `LocalTime.PlusMinutes` | `strftime('%H:%M:%f', <column>, '+n minutes')` |
| `LocalTime.PlusSeconds` | `strftime('%H:%M:%f', <column>, '+n seconds')` |
| `LocalTime.PlusMilliseconds` | `strftime('%H:%M:%f', <column>, '+0.n seconds')` |
| `LocalTime.ToTimeOnly` | `strftime('%H:%M:%f', <column>)` |
| `LocalDateTime.PlusYears` | `strftime('%Y-%m-%d %H:%M:%f',<column>, '+n years')` |
| `LocalDateTime.PlusMonths` | `strftime('%Y-%m-%d %H:%M:%f',<column>, '+n months')` |
| `LocalDateTime.PlusWeeks` | `strftime('%Y-%m-%d %H:%M:%f',<column>, '+n*7 days')` |
| `LocalDateTime.PlusDays` | `strftime('%Y-%m-%d %H:%M:%f',<column>, '+n days')` |
| `LocalDateTime.PlusHours` | `strftime('%Y-%m-%d %H:%M:%f', <column>, '+n hours')` |
| `LocalDateTime.PlusMinutes` | `strftime('%Y-%m-%d %H:%M:%f', <column>, '+n minutes')` |
| `LocalDateTime.PlusSeconds` | `strftime('%Y-%m-%d %H:%M:%f', <column>, '+n seconds')` |
| `LocalDateTime.PlusMilliseconds` | `strftime('%Y-%m-%d %H:%M:%f', <column>, '+0.n seconds')` |
| `LocalDateTime.ToDateTimeUnspecified` | `strftime('%Y-%m-%d %H:%M:%f', <column>)` |

When these methods are chained, all modifiers will be added to the same function call, like this:

```csharp
context.NodaTime.Select(x => x.LocalDateTime.PlusMonths(2).PlusDays(2).PlusHours(2).PlusSeconds(2))
```

Results in the following SQL:

```sql
SELECT strftime('%Y-%m-%d %H:%M:%f', "n"."LocalDateTime", '+2 months', '+2 days', '+2 hours', '+2 seconds')
```

## Sponsors

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=khellang&utm_medium=EFCore.Sqlite.NodaTime) and [Dapper Plus](https://dapper-plus.net/?utm_source=khellang&utm_medium=EFCore.Sqlite.NodaTime) are major sponsors and proud to contribute to the development of EFCore.Sqlite.NodaTime.

[![Entity Framework Extensions](https://raw.githubusercontent.com/khellang/EFCore.Sqlite.NodaTime/master/entity-framework-extensions-major-sponsor.png)](https://entityframework-extensions.net/bulk-insert?utm_source=khellang&utm_medium=EFCore.Sqlite.NodaTime)

[![Dapper Plus](https://raw.githubusercontent.com/khellang/EFCore.Sqlite.NodaTime/master/dapper-plus-sponsor.png)](https://dapper-plus.net/bulk-insert?utm_source=khellang&utm_medium=EFCore.Sqlite.NodaTime)
