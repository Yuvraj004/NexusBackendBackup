namespace NexusBackend.DTOs;

public record RegisterRequest(
    string UserName,
    string Email,
    string Password
);

public record LoginRequest(
    string Email,
    string Password
);

public record LoginResponse(
    int Id,
    string UserName,
    string Email,
    string Message
);