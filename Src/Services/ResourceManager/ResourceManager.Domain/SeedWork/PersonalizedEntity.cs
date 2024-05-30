using SharedKernel.Domain;

namespace ResourceManager.Domain.SeedWork;

public abstract class PersonalizedEntity :  EntityAuditBase, IPersonalizeEntity
{
    public string? OwnerId { get; set; }
}
