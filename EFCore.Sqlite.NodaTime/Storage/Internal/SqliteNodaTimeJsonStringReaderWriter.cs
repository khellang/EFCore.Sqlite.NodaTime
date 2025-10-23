using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.Json;
using NodaTime.Text;

#pragma warning disable EF9100

namespace Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;

public class SqliteNodaTimeJsonStringReaderWriter<T> : JsonValueReaderWriter<T> where T : struct
{
    private readonly IPattern<T> _pattern;

    public SqliteNodaTimeJsonStringReaderWriter()
    {
        ConstructorExpression = CreateConstructorExpression(out _pattern);
    }

    // ReSharper disable once UnusedMember.Global
    public SqliteNodaTimeJsonStringReaderWriter(IPattern<T> pattern)
    {
        _pattern = pattern;
        ConstructorExpression = null!;
    }

    public override Expression ConstructorExpression { get; }

    public override T FromJsonTyped(ref Utf8JsonReaderManager manager, object? existingObject = null)
    {
        return _pattern.Parse(manager.CurrentReader.GetString()!).Value;
    }

    public override void ToJsonTyped(Utf8JsonWriter writer, T value)
    {
        writer.WriteStringValue(_pattern.Format(value));
    }

    private static NewExpression CreateConstructorExpression(out IPattern<T> pattern)
    {
        const BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        // SqlitePatterns should contain field named after T, e.g. LocalDate
        var field = typeof(SqlitePatterns).GetField(typeof(T).Name, flags);
        Debug.Assert(field?.GetValue(null) is IPattern<T>);
        pattern = (IPattern<T>)field.GetValue(null)!;

        // Create constructor expression: Use static property rather than constant to avoid capturing
        // Result: new SqliteNodaTimeJsonStringReaderWriter<T>(SqlitePatterns.<patternName>)
        var constructor = typeof(SqliteNodaTimeJsonStringReaderWriter<T>).GetConstructor([typeof(IPattern<T>)]);
        Debug.Assert(constructor != null);
        return Expression.New(constructor, Expression.Field(null, field));
    }
}
