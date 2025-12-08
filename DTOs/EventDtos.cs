namespace NexusBackend.DTOs;

public record EventDto(
    int Id,
    string Name,
    DateTimeOffset Date
);

public record CreateEventRequest(
    string Name,
    DateTimeOffset Date
);