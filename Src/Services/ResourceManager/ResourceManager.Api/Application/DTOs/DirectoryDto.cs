using Newtonsoft.Json;

namespace ResourceManager.Api.Application.DTOs;

public class DirectoryDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public Guid? ParentId { get; set; }
    
    public int ChildrenCount { get; set; }
    
    public string Path { get; set; }

    public bool IsLocked { get; set; }

    public bool HasPassword { get; set; }
    
    public DateTime CreatedDate { get; set; }

    public DateTime? LastModifiedDate { get; set; }
    
}
