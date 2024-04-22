using SharedKernel.Contracts;
using Enum = System.Enum;

namespace Identity.Infrastructure.Services;

public class AuthService : IAuthService
{
    public bool CheckPermission(ActionExponent exponent)
    {
        throw new NotImplementedException();
    }

    public bool CheckPermission(ActionExponent[] exponents)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GenerateAccessTokenAsync(TokenUser token, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public string GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }

    public async Task RevokeAccessTokenAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CheckRefreshTokenAsync(string value, Guid ownerId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}