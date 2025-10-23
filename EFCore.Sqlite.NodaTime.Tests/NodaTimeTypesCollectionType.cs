using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore.Sqlite;

/// <summary>
/// Entity that contains collections of NodaTime types
/// </summary>
public class NodaTimeTypesCollectionType<T> where T : struct
{
    public int Id { get; set; }

    public required T[] Array { get; set; }
    public required List<T> List { get; set; }
    public required IList<T> IList { get; set; }
    public required IReadOnlyList<T> IReadOnlyList { get; set; }

    public required T?[] ArrayNullable { get; set; }
    public required List<T?> ListNullable { get; set; }
    public required IList<T?> IListNullable { get; set; }
    public required IReadOnlyList<T?> IReadOnlyListNullable { get; set; }
}
