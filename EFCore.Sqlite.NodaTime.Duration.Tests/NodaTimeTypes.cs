using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite;

public class NodaTimeTypes
{
    public int Id { get; set; }
    public LocalDateTime LocalDateTime { get; set; }
    public Instant Instant { get; set; }
    public Duration Duration { get; set; }
}
