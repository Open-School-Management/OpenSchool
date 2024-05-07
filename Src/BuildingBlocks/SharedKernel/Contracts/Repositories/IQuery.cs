namespace SharedKernel.Contracts.Repositories;

public interface IQuery<T>
{
    IQueryable<T> Query();
}
