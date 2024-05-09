using Identity.Application.Persistence;

namespace Identity.Application.Repositories;

public interface IUserRepository : IWriteOnlyRepository<User, Guid>
{
    
}