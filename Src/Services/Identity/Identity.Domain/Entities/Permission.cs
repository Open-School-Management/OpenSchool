namespace Identity.Domain.Entities;

[Table(TableName.Permission)]
public class Permission : EntityAuditBase
{
    public string Code { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public int Exponent { get; set; }
    
    #region Navigations
    
    public ICollection<RolePermission> RolePermissions { get; set; }
    public ICollection<UserPermission> UserPermissions { get; set; }
    
    #endregion
    
    
}