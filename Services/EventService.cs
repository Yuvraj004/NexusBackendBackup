using Microsoft.EntityFrameworkCore;
using NexusBackend.Data;
using NexusBackend.DTOs;
using NexusBackend.Entities;

namespace NexusBackend.Services;

public class EventService : IEventService
{
    private readonly NexusDbContext _context;

    public EventService(NexusDbContext context)
    {
        _context = context;
    }

    public async Task<List<EventDto>> GetAllEventsAsync()
    {
        return await _context.Events
            .OrderBy(e => e.Date)
            .Select(e => new EventDto(e.Id, e.Name, e.Date))
            .ToListAsync();
    }

    public async Task<EventDto> CreateEventAsync(CreateEventRequest request)
    {
        // 1. Create the Event
        var newEvent = new Event
        {
            Name = request.Name,
            Date = request.Date
        };

        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync(); // Save to get the new Event.Id

        // 2. THE SENIOR DEV MOVE: Auto-Seed Seats
        // Instead of an empty event, let's fill it with a 5x10 grid (50 seats)
        var seats = new List<Seat>();
        string[] rows = { "A", "B", "C", "D", "E" };

        foreach (var row in rows)
        {
            for (var number = 1; number <= 10; number++)
            {
                seats.Add(new Seat
                {
                    EventId = newEvent.Id,
                    SeatNumber = $"{row}{number}", // e.g., "A1", "A2"...
                    Status = SeatStatus.Available
                });
            }
        }

        _context.Seats.AddRange(seats);
        await _context.SaveChangesAsync();

        return new EventDto(newEvent.Id, newEvent.Name, newEvent.Date);
    }
}