using Microsoft.EntityFrameworkCore;
using NexusBackend.Data;
using NexusBackend.DTOs;
using NexusBackend.Entities;
using BCrypt.Net; // Import the hashing tool

namespace NexusBackend.Services;

public class AuthService : IAuthService
{
    private readonly NexusDbContext _context;

    public AuthService(NexusDbContext context)
    {
        _context = context;
    }

    public async Task<LoginResponse?> RegisterAsync(RegisterRequest request)
    {
        // 1. Validation: Does email already exist?
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            return null; // Email taken
        }

        // 2. Security: Hash the password
        // NEVER store the plain text password from the request.
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // 3. Create User
        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,
            Password = passwordHash // Store the hash
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new LoginResponse(user.Id, user.UserName!, user.Email, "Registration Successful");
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        // 1. Find User by Email
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
        {
            return null; // User not found
        }

        // 2. Verify Password
        // We compare the incoming plain text against the stored hash.
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

        if (!isPasswordValid)
        {
            return null; // Wrong password
        }

        return new LoginResponse(user.Id, user.UserName!, user.Email, "Login Successful");
    }
}