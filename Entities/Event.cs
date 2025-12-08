using System.ComponentModel.DataAnnotations;

namespace NexusBackend.Entities;

public class Event
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    // Always use DateTimeOffset for distributed systems
    public DateTimeOffset Date { get; set; }

    // Navigation property (Optional, but helpful for queries)
    public ICollection<Seat> Seats { get; set; } = new List<Seat>();
}