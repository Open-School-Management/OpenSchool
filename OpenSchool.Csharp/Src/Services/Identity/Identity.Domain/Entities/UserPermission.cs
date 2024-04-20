namespace Identity.Domain.Entities;

[Table(TableName.UserConfig)]
public class UserPermission : EntityAuditBase
{
    #region Relationships

    public Guid UserId { get; set; }
    public Guid ActionId { get; set; }

    #endregion
    
    #region Navigations
    
    public virtual User User { get; set; }
    public virtual Permission Permission { get; set; }
    
    #endregion
}