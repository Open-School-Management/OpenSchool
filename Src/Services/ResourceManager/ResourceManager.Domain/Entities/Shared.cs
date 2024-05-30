using System.ComponentModel.DataAnnotations.Schema;
using ResourceManager.Domain.Enums;
using SharedKernel.Domain;

namespace ResourceManager.Domain.Entities;

[Table("shared_directory")]
public class Shared : EntityAuditBase
{
    public Guid DirectoryId { get; set; }
    public Guid OwnerId { get; set; }
    public Guid UserId { get; set; }
    public DateTime? ExpirationDate { get; set; } // Ngày hết hạn
    public PermissionType PermissionType { get; set; }
    
    #region Navigations
    
    public virtual Directory Directory { get; set; }
    
    #endregion
}