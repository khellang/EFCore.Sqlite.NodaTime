using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NodaTime;
// ReSharper disable UnusedParameter.Global

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore;

[ExcludeFromCodeCoverage]
public static class SqliteNodaTimeDurationDbFunctionsExtensions
{
    /// <summary>
    /// Calculates the duration between two <see cref="Instant" /> values.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="start">The earlier instant value</param>
    /// <param name="end">The later instant value</param>
    public static Duration DurationBetween(this DbFunctions _, Instant start, Instant end)
        => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(DurationBetween)));
}
