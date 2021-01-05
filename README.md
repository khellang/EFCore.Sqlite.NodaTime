# EFCore.Sqlite.NodaTime

![Build](https://github.com/khellang/EFCore.Sqlite.NodaTime/workflows/Build/badge.svg)

Adds support for [NodaTime](https://github.com/nodatime/nodatime) types when using [SQLite](https://sqlite.org/) with [Entity Framework Core](https://github.com/dotnet/efcore).

## Installation

![Nuget](https://img.shields.io/nuget/v/EFCore.Sqlite.NodaTime)

Install the latest package from [NuGet](https://www.nuget.org/package/EFCore.Sqlite.NodaTime).

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