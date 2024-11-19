using NodaTime;

namespace Microsoft.EntityFrameworkCore.Sqlite;

public class NodaTimeTypes
{
    public int Id { get; set; }
    public Instant Instant { get; set; }
    public LocalTime LocalTime { get; set; }
    public LocalDate LocalDate { get; set; }
    public LocalDateTime LocalDateTime { get; set; }
}