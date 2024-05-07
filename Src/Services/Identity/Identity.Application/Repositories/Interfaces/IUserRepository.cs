using Identity.Application.DTOs.User;
using Identity.Application.Persistence;

namespace Identity.Application.Repositories.Interfaces;

public interface IUserRepository : IWriteOnlyRepository<User, Guid, IIdentityDbContext>
{
    Task<UserDto> GetUserInformationAsync(Guid userId, CancellationToken cancellationToken = default);
    
    Task<string> CheckDuplicateAsync(string username, string email, string phone, CancellationToken cancellationToken = default);
    
    Task<User> CreateUserAsync(User user,CancellationToken cancellationToken = default);
    
    Task<User> UpdateUserAsync(User user,CancellationToken cancellationToken = default);
    
    Task<bool> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default);
}