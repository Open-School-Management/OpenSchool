namespace Identity.Domain.Entities;

[Table(TableName.MFA)]
public class MFA : EntityAuditBase
{
    public MFAType Type { get; set; } = MFAType.None;

    public bool Enabled { get; set; }

    #region Relationships

    public Guid UserId { get; set; }

    #endregion
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
}