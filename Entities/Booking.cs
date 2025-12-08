namespace NexusBackend.Entities;

public class Booking
{
    public int Id { get; set; }

    public int SeatId { get; set; }

    public int UserId { get; set; }

    // Navigation Property
    public User User { get; set; } = null!;

    public DateTimeOffset BookedAt { get; set; } = DateTimeOffset.UtcNow;

    // For Idempotency (preventing double-charges)
    public Guid? IdempotencyKey { get; set; }
}