using Microsoft.EntityFrameworkCore;
using NexusBackend.Data;
using NexusBackend.DTOs;
using NexusBackend.Entities;
using Hangfire;

namespace NexusBackend.Services;

public class SeatService : ISeatService
{
    private readonly NexusDbContext _context;
    private readonly IBackgroundJobClient _backgroundJobClient; // Inject Hangfire Client

    public SeatService(NexusDbContext context, IBackgroundJobClient backgroundJobClient)
    {
        _context = context;
        _backgroundJobClient = backgroundJobClient;
    }

    public async Task<List<SeatDto>> GetSeatsAsync(int eventId)
    {
        return await _context.Seats
            .Where(s => s.EventId == eventId)
            .Select(s => new SeatDto(
                s.Id,
                s.SeatNumber,
                s.Status.ToString(),
                s.CurrentUserId))
            .ToListAsync();
    }

    public async Task<bool> ReserveSeatAsync(int seatId, int userId)
    {
        // 1. FAST READ: Fetch the seat
        // We track it so EF Core can watch for changes
        var seat = await _context.Seats.FindAsync(seatId);

        // 2. LOGIC CHECK: Is it already gone?
        if (seat == null || seat.Status != SeatStatus.Available)
        {
            return false; // Fail immediately
        }

        // 3. PREPARE UPDATE: Set the new state
        seat.Status = SeatStatus.Held;
        seat.CurrentUserId = userId;
        seat.HoldExpiry = DateTimeOffset.UtcNow.AddMinutes(5); // Hold for 5 mins

        // NOTE: We do NOT touch 'RowVersion'. 
        // SQL Server and EF Core handle that checking automatically.

        try
        {
            // 4. ATOMIC COMMIT: This is where the magic happens.
            // EF Core generates: UPDATE Seats SET ... WHERE Id=x AND RowVersion=oldVersion
            await _context.SaveChangesAsync();
            _backgroundJobClient.Schedule<ISeatCleanupService>(
                service => service.ReleaseSeatIfExpiredAsync(seat.Id),
                TimeSpan.FromMinutes(5));
            return true; // Success!
        }
        catch (DbUpdateConcurrencyException)
        {
            // 5. CATCH THE RACE CONDITION
            // This block runs if User B modified the row while we were processing.
            // The 'RowVersion' in DB didn't match what we read in Step 1.
            return false;
        }
    }
}