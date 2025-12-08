using Microsoft.EntityFrameworkCore;
using NexusBackend.Data;
using NexusBackend.Entities;

namespace NexusBackend.Services;

public class SeatCleanupService : ISeatCleanupService
{
    private readonly NexusDbContext _context;

    public SeatCleanupService(NexusDbContext context)
    {
        _context = context;
    }

    public async Task ReleaseSeatIfExpiredAsync(int seatId)
    {
        var seat = await _context.Seats.FindAsync(seatId);

        // Safety Checks
        if (seat == null) return;

        // If the user already paid (Status == Sold), DO NOT release it!
        if (seat.Status == SeatStatus.Sold) return;

        // If the seat is already free, do nothing.
        if (seat.Status == SeatStatus.Available) return;

        // CRITICAL: Check if time actually expired (Double check safety)
        if (seat.HoldExpiry > DateTimeOffset.UtcNow) return;

        // Logic: Release the seat
        seat.Status = SeatStatus.Available;
        seat.CurrentUserId = null;
        seat.HoldExpiry = null;

        // We don't need to touch RowVersion; EF handles it.
        await _context.SaveChangesAsync();

        Console.WriteLine($"[Hangfire] Released Seat {seat.SeatNumber} because timer expired.");
    }
}