using SharedKernel.Domain;

namespace ResourceManager.Domain.SeedWork;

public abstract class PersonalizedEntity :  EntityAuditBase, IPersonalizeEntity
{
    public Guid? OwnerId { get; set; }
}
