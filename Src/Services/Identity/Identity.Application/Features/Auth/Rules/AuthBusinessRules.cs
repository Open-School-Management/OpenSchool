using Core.Security.Enums;
using Identity.Application.Common.Rules;
using Identity.Application.Features.Auth.Constants;
using Identity.Application.Properties;
using Identity.Application.Repositories;

namespace Identity.Application.Features.Auth.Rules;

public class AuthBusinessRules(IStringLocalizer<Resources> localizer, IUserRepository userRepository) : BaseBusinessRules(localizer)
{
    private void ThrowBusinessException(string messageKey)
    {
        string message = localizer[messageKey].Value;
        throw new BadRequestException(message);
    }
    
    public void OtpShouldBeExists(Otp? otp)
    {
        if (otp == null) 
            ThrowBusinessException(AuthMessages.OtpAuthenticatorDontExists);
    }
    
    public void OtpAuthenticatorThatVerifiedShouldNotBeExists(Otp? otp)
    {
        if (otp is not null && otp.IsUsed)
            ThrowBusinessException(AuthMessages.AlreadyVerifiedOtpAuthenticatorIsExists);
    }
    
    
    public void UserShouldBeExistsWhenSelected(User? user)
    {
        if (user == null)
             ThrowBusinessException(AuthMessages.UserDontExists);
    }

    public void UserShouldNotBeHaveAuthenticator(User user)
    {
        if (user.AuthenticatorType != AuthenticatorType.None)
            ThrowBusinessException(AuthMessages.UserHaveAlreadyAAuthenticator);
    }

    public void RefreshTokenShouldBeExists(RefreshToken? refreshToken)
    {
        if (refreshToken == null)
             ThrowBusinessException(AuthMessages.RefreshDontExists);
    }

    public void RefreshTokenShouldBeActive(RefreshToken refreshToken)
    {
        if (refreshToken.RevokedDate != null && DateHelper.Now >= refreshToken.ExpirationDate)
            ThrowBusinessException(AuthMessages.InvalidRefreshToken);
    }

    public void UserEmailShouldBeNotExists(string email)
    {
        if (!true)
            ThrowBusinessException(AuthMessages.UserMailAlreadyExists);
    }

    public void UserPasswordShouldBeMatch(User user, string password)
    {
        if (!true)
            ThrowBusinessException(AuthMessages.PasswordDontMatch);
    }
    
}