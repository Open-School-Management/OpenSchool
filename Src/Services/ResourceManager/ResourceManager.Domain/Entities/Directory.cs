using System.ComponentModel.DataAnnotations.Schema;
using ResourceManager.Domain.SeedWork;

namespace ResourceManager.Domain.Entities;

[Table("directory")]
public class Directory : PersonalizedEntity
{
    public string Name { get; set; }

    public Guid? ParentId { get; set; }

    public string Path { get; set; }

    public int DuplicateNo { get; set; }
    
    #region Navigations
    
    public virtual Directory? Parent { get; set; }
    
    public virtual LockedDirectory? LockedDirectory { get; set; }
    
    public ICollection<File>? Files { get; set; }
    
    public ICollection<Shared>? Shareds { get; set; }
    
    #endregion
}

