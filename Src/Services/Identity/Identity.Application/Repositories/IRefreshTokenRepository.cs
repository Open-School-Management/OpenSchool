namespace Identity.Application.Repositories;

public interface IRefreshTokenRepository : IWriteOnlyRepository<RefreshToken, Guid>
{
    
}