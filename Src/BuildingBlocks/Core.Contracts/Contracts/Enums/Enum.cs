using System.ComponentModel;

namespace SharedKernel.Contracts;

public static class Enum
{
    public enum GenderType
    {
        [Description("Other")] Other = 0,

        [Description("Female")] Female = 1,

        [Description("Male")] Male = 2,
    }

    public enum HttpStatusCodeExtension
    {
        AuthenticationRequired = 432,
        NotVerified = 801,
    }

    public enum EntityState
    {
        Add = 1,
        Edit = 2,
        Delete = 3
    }

    public enum ChangeType
    {
        Add = 1,
        Edit = 2,
        Delete = 3,
    }

    public enum ValidateCode : int
    {
        Required = 1,
        Duplicate = 2,
        Invalid = 3,
        EntityNull = 4,
    }

    public enum RoleId : long
    {
        SA = 1,
        ADMIN = 2,
        EMPLOYEE = 3
    }
    public enum FileType
    {
        None = 0,
        Image = 1,
        Video = 2,
        Other = 3,
        Document = 4,
    }
}