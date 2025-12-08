using Microsoft.EntityFrameworkCore;
using NexusBackend.Entities;

namespace NexusBackend.Data;

public class NexusDbContext : DbContext
{
    public NexusDbContext(DbContextOptions<NexusDbContext> options) : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Performance Indexing
        modelBuilder.Entity<Seat>()
            .HasIndex(s => s.EventId);


        var eventDate = new DateTimeOffset(
                year: 2026, month: 01, day: 15,
                hour: 09, minute: 00, second: 00,
                offset: TimeSpan.Zero);

        // Seed Data (Optional: Creates 1 Event and 5 Seats for testing)
        modelBuilder.Entity<Event>().HasData(
            new Event { Id = 1, Name = "Tech Conference 2025", Date = eventDate }
        );


        // Relationship: A Seat can be held by one User (optional)
        modelBuilder.Entity<Seat>()
            .HasOne(s => s.CurrentUser)
            .WithMany() // User can hold many seats, but we don't strictly need a collection on User for this
            .HasForeignKey(s => s.CurrentUserId)
            .OnDelete(DeleteBehavior.SetNull); // If user is deleted, release the seat

        // Relationship: A Booking must belong to a User
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Don't delete history if user is deleted

        modelBuilder.Entity<Seat>().HasData(
            new Seat { Id = 1, EventId = 1, SeatNumber = "1A" },
            new Seat { Id = 2, EventId = 1, SeatNumber = "1B" },
            new Seat { Id = 3, EventId = 1, SeatNumber = "1C" },
            new Seat { Id = 4, EventId = 1, SeatNumber = "1D" },
            new Seat { Id = 5, EventId = 1, SeatNumber = "1E" }
        );
    }
}