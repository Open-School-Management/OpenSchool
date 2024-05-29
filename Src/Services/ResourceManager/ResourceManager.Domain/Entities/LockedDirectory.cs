using System.ComponentModel.DataAnnotations.Schema;
using ResourceManager.Domain.SeedWork;

namespace ResourceManager.Domain.Entities;

[Table("locked_directory")]
public class LockedDirectory : PersonalizedEntity
{
    public Guid DirectoryId { get; set; }

    public bool EnabledLock { get; set; }

    public string Password { get; set; }
    
    #region Navigations
    public virtual Directory Directory { get; set; }
    
    #endregion
}