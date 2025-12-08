using System.ComponentModel.DataAnnotations;

namespace NexusBackend.Entities;

public enum SeatStatus
{
    Available = 0,
    Held = 1,
    Sold = 2
}

public class Seat
{
    public int Id { get; set; }

    // Foreign Key
    public int EventId { get; set; }

    [Required]
    [MaxLength(10)]
    public string SeatNumber { get; set; } = string.Empty;

    public SeatStatus Status { get; set; } = SeatStatus.Available;

    // Optimization: Track owner directly on the seat to avoid Joins
    public int? CurrentUserId { get; set; }

    // Navigation Property
    // This allows you to access the User object directly: seat.CurrentUser.Email
    public User? CurrentUser { get; set; }

    // Optimization: Expiry time for the "Hold" background job
    public DateTimeOffset? HoldExpiry { get; set; }

    // ---------------------------------------------------------
    // THE SENIOR DEV MOVE: SQL Server RowVersion
    // ---------------------------------------------------------
    // This maps to a binary column that changes automatically on every update.
    // EF Core uses this to detect race conditions.
    [Timestamp]
    public byte[] RowVersion { get; set; }
}