using NexusBackend.DTOs;

namespace NexusBackend.Services;

public interface IEventService
{
    Task<List<EventDto>> GetAllEventsAsync();
    Task<EventDto> CreateEventAsync(CreateEventRequest request);
}