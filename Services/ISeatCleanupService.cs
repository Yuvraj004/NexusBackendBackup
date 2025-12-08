namespace NexusBackend.Services;

public interface ISeatCleanupService
{
    Task ReleaseSeatIfExpiredAsync(int seatId);
}