using FinApp.Domain.Entities;

namespace FinApp.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByUsernameWithRoleAsync(string username);
    Task<User?> GetByIdWithRoleAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> UsernameExistsAsync(string username);
}
