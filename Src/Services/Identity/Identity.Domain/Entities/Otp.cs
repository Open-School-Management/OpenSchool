namespace Identity.Domain.Entities;

[Table(TableName.OTP)]
public class Otp : PersonalizedEntityAuditBase
{
    public string Code { get; set; }

    public bool IsUsed { get; set; }

    public DateTime ExpiredDate { get; set; }

    
    public DateTime ProvidedDate { get; set; }
    

    public OtpType Type { get; set; } = OtpType.None;
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
}