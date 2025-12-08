namespace NexusBackend.DTOs;

// What the frontend sees
public record SeatDto(int Id, string SeatNumber, string Status, int? CurrentUserId);

// What the frontend sends to book
public record BookSeatRequest(int SeatId, int UserId);
