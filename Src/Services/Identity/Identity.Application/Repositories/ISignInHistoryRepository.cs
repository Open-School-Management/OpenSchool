namespace Identity.Application.Repositories;

public interface ISignInHistoryRepository : IWriteOnlyRepository<SignInHistory, Guid>
{
    
}