using System.ComponentModel.DataAnnotations.Schema;
using SharedKernel.Domain;
using ResourceManager.Domain.Enums;
using ResourceManager.Domain.SeedWork;

namespace ResourceManager.Domain.Entities;

[Table("recycle_bin")]
public class RecycleBin : PersonalizedEntity
{
    public Guid ResourceId { get; set; }
    
    public ResourceType ResourceType { get; set; }
    
    public DateTime RestoredAt { get; set; }
}