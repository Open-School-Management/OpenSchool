using SharedKernel.Domain;

namespace ResourceManager.Domain.SeedWork;

public class PersonalizedEntity :  EntityAuditBase, IPersonalizeEntity
{
    public Guid? OwnerId { get; set; }
}
