namespace SharedKernel.Domain;

public interface IEntityBase<T> : ICoreEntity
{
    T Id { get; set; }
}

public interface IEntityBase : IEntityBase<Guid>
{
    
}