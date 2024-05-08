namespace Identity.Application.Features.Auth.Constants;

public class AuthMessages
{
    public const string EmailAuthenticatorDontExists = "EmailAuthenticatorDontExists";
    public const string OtpAuthenticatorDontExists = "OtpAuthenticatorDontExists";
    public const string AlreadyVerifiedOtpAuthenticatorIsExists = "AlreadyVerifiedOtpAuthenticatorIsExists";
    public const string EmailActivationKeyDontExists = "EmailActivationKeyDontExists";
    public const string UserDontExists = "UserDontExists";
    public const string UserHaveAlreadyAAuthenticator = "UserHaveAlreadyAAuthenticator";
    public const string RefreshDontExists = "RefreshDontExists";
    public const string InvalidRefreshToken = "InvalidRefreshToken";
    public const string UserMailAlreadyExists = "UserMailAlreadyExists";
    public const string PasswordDontMatch = "PasswordDontMatch";
}