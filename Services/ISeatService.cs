using NexusBackend.DTOs;

namespace NexusBackend.Services;

public interface ISeatService
{
    Task<List<SeatDto>> GetSeatsAsync(int eventId);
    Task<bool> ReserveSeatAsync(int seatId, int userId);
}