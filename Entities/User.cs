using System.ComponentModel.DataAnnotations;

namespace NexusBackend.Entities;

public class User
{
    public int Id { get; set; }

    [MaxLength(200)]
    public string? UserName { get; set; } // Matches VARCHAR(200)

    [Required]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Password { get; set; } = string.Empty; // Ideally store a Hash here

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    // Navigation Properties (Optional but useful for "Get My Bookings")
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}