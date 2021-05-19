using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NodaTime;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    [ExcludeFromCodeCoverage]
    public static class SqliteNodaTimeDbFunctionsExtensions
    {
        public static int DateDiffYear(this DbFunctions _, LocalDate startDate, LocalDate endDate)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffYear)));

        public static int? DateDiffYear(this DbFunctions _, LocalDate? startDate, LocalDate? endDate)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffYear)));

        public static int DateDiffYear(this DbFunctions _, LocalDateTime startDateTime, LocalDateTime endDateTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffYear)));

        public static int? DateDiffYear(this DbFunctions _, LocalDateTime? startDateTime, LocalDateTime? endDateTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffYear)));

        public static int DateDiffMonth(this DbFunctions _, LocalDate startDate, LocalDate endDate)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffMonth)));

        public static int? DateDiffMonth(this DbFunctions _, LocalDate? startDate, LocalDate? endDate)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffMonth)));

        public static int DateDiffMonth(this DbFunctions _, LocalDateTime startDateTime, LocalDateTime endDateTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffMonth)));

        public static int? DateDiffMonth(this DbFunctions _, LocalDateTime? startDateTime, LocalDateTime? endDateTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffMonth)));

        public static int DateDiffWeek(this DbFunctions _, LocalDate startDate, LocalDate endDate)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffWeek)));

        public static int? DateDiffWeek(this DbFunctions _, LocalDate? startDate, LocalDate? endDate)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffWeek)));

        public static int DateDiffWeek(this DbFunctions _, LocalDateTime startDateTime, LocalDateTime endDateTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffWeek)));

        public static int? DateDiffWeek(this DbFunctions _, LocalDateTime? startDateTime, LocalDateTime? endDateTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffWeek)));

        public static int DateDiffDay(this DbFunctions _, LocalDate startDate, LocalDate endDate)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffDay)));

        public static int? DateDiffDay(this DbFunctions _, LocalDate? startDate, LocalDate? endDate)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffDay)));

        public static int DateDiffDay(this DbFunctions _, LocalDateTime startDateTime, LocalDateTime endDateTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffDay)));

        public static int? DateDiffDay(this DbFunctions _, LocalDateTime? startDateTime, LocalDateTime? endDateTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffDay)));

        public static int DateDiffHour(this DbFunctions _, LocalDateTime startDateTime, LocalDate endDateTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffHour)));

        public static int? DateDiffHour(this DbFunctions _, LocalDateTime? startDateTime, LocalDateTime? endDateTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffHour)));

        public static int DateDiffHour(this DbFunctions _, LocalTime startTime, LocalTime endTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffHour)));

        public static int? DateDiffHour(this DbFunctions _, LocalTime? startTime, LocalTime? endTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffHour)));

        public static int DateDiffMinute(this DbFunctions _, LocalDateTime startDateTime, LocalDateTime endDateTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffMinute)));

        public static int? DateDiffMinute(this DbFunctions _, LocalDateTime? startDateTime, LocalDateTime? endDateTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffMinute)));

        public static int DateDiffMinute(this DbFunctions _, LocalTime startTime, LocalTime endTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffMinute)));

        public static int? DateDiffMinute(this DbFunctions _, LocalTime? startTime, LocalTime? endTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffMinute)));

        public static int DateDiffSecond(this DbFunctions _, LocalDateTime startDateTime, LocalDateTime endDateTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffSecond)));

        public static int? DateDiffSecond(this DbFunctions _, LocalDateTime? startDateTime, LocalDateTime? endDateTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffSecond)));

        public static int DateDiffSecond(this DbFunctions _, LocalTime startTime, LocalTime endTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffSecond)));

        public static int? DateDiffSecond(this DbFunctions _, LocalTime? startTime, LocalTime? endTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffSecond)));

        public static int DateDiffMillisecond(this DbFunctions _, LocalDateTime startDateTime, LocalDateTime endDateTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffMillisecond)));

        public static int? DateDiffMillisecond(this DbFunctions _, LocalDateTime? startDateTime, LocalDateTime? endDateTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffMillisecond)));

        public static int DateDiffMillisecond(this DbFunctions _, LocalTime startTime, LocalTime endTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffMillisecond)));

        public static int? DateDiffMillisecond(this DbFunctions _, LocalTime? startTime, LocalTime? endTime)
            => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DateDiffMillisecond)));
    }
}
