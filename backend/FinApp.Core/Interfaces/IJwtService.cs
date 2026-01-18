using FinApp.Domain.Entities;

namespace FinApp.Core.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
    int? ValidateToken(string token);
    string? GetUserIdFromToken(string token);
}
