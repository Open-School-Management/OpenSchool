using System.ComponentModel.DataAnnotations.Schema;
using ResourceManager.Domain.SeedWork;

namespace ResourceManager.Domain.Entities;

[Table("resource_config")]
public class Config : PersonalizedEntity
{
    public long MaxCapacity { get; set; }
    public long MaxFileSize { get; set; }
    public long CurrentUsage { get; set; }
}