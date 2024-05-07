using System.ComponentModel;

namespace Core.Security.Enums;

public class SecurityEnum
{
    public enum OtpType
    {
        [Description("None")] None = 0,

        [Description("For password")] Password = 1,

        [Description("For verify")] Verify = 2,

        [Description("Multi-factor Authentication")]
        MFA = 3,
    }

    public enum MFAType
    {
        [Description("None")] None = 0,

        [Description("Use email")] Email = 1,

        [Description("Use phonenumber")] Phone = 2,
    }

    public enum AccountState
    {
        [Description("Actived")] Actived = 1,

        [Description("NotActived")] NotActived = 2,

        [Description("Blocked")] Blocked = 3,
    }
    
    public enum ActionExponent : int
    {
        AllowAnonymous = -1,
        SupperAdmin = 128,
        Admin = 64,
        View = 0,
        Add = 1,
        Edit = 2,
        Delete = 3,
        Export = 4,
        Import = 5,
        Upload = 6,
        Download = 7,
        Update = 8,
    }
    
    public enum StatusType
    {
        Active = 0,
        InActive = 1,
    }
}