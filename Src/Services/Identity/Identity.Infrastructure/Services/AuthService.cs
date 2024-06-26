using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Caching.Enums;
using Caching.Sequence;
using Core.Security.Constants;
using Core.Security.Models;
using Core.Security.Services.Auth;
using Core.Security.Utilities;

namespace Identity.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly ISequenceCaching _caching;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceProvider _provider;
    public AuthService(
        IAuthRepository authRepository,
        ISequenceCaching caching,
        IHttpContextAccessor httpContextAccessor,
        IServiceProvider provider)
    {
        _authRepository = authRepository;
        _caching = caching;
        _httpContextAccessor = httpContextAccessor;
        _provider = provider;
    }
    
    public bool CheckPermission(ActionExponent exponent, string permission)
    {
        return CheckPermission(new ActionExponent[] { exponent }, permission);
    }

    public bool CheckPermission(ActionExponent[] exponents, string permission)
    {
        var length = exponents.Length;
        for (int i = 0; i < length; i++)
        {
            var action = AuthUtility.FromExponentToPermission((int)exponents[i]);
            if (!AuthUtility.ComparePermissionAsString(permission, action))
            {
                return false;
            }
        }
        return true;
    }

    public async Task<string> GenerateAccessTokenAsync(TokenUser token, CancellationToken cancellationToken = default)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var secretKey = Encoding.UTF8.GetBytes(DefaultJWTConfig.Key);
        var symmetricSecurityKey = new SymmetricSecurityKey(secretKey);
        var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
        var claims = new List<Claim>();
        
        // Add claims for access token
        claims.Add(new Claim(ClaimConstant.USER_ID, token.Id.ToString()));
        claims.Add(new Claim(ClaimConstant.USERNAME, token.Username));
        claims.Add(new Claim(ClaimConstant.ROLES, string.Join(",", token.RoleNames)));
        claims.Add(new Claim(ClaimConstant.PERMISSION, token.Permission));
        claims.Add(new Claim(ClaimConstant.CREATE_AT, DateHelper.Now.ToString("MM/dd/yyyy HH:mm:ss")));
        claims.Add(new Claim(ClaimConstant.AUTHOR, "Đỗ Chí Hùng"));
        claims.Add(new Claim(ClaimConstant.ORGANIZATION, "Open-School Microservices"));
        claims.Add(new Claim(ClaimConstant.AUTHORS_MESSAGE, "Contact for work: 0976580418; Facebook: https://www.facebook.com/dohung6924"));
        
        var securityToken = new JwtSecurityToken(
            issuer: DefaultJWTConfig.Issuer,
            audience: DefaultJWTConfig.Audience,
            claims: claims,
            expires: DateHelper.Now.AddSeconds(DefaultJWTConfig.ExpiredSecond),
            signingCredentials: credentials
        );
        
        var accessToken = jwtSecurityTokenHandler.WriteToken(securityToken);
        /*
         * Save token vào redis caching
         * Nếu chỉ cho phép online trên 1 thiết bị: revoke (thu hồi) token cũ, save token mới
         * Nếu cho phép online trên nhiều thiết bị: update token
         */
        
        var key = AuthCacheKeys.GetAccessTokenKey(token.Id);
        var oldTokens = await _caching.GetStringAsync(key);
        if (CoreSettings.IsSingleDevice)
        {
            if (!string.IsNullOrEmpty(oldTokens))
            {
                var tokens = oldTokens.Split(";");
                await _authRepository.RemoveRefreshTokenAsync(cancellationToken);
                await Task.WhenAll(tokens.Select(token => RevokeAccessTokenAsync(token, cancellationToken)).Concat(new Task[] { _caching.DeleteAsync(key) }));
                
                await _caching.SetAsync(key, accessToken, TimeSpan.FromSeconds(DefaultJWTConfig.ExpiredSecond), onlyUseType: CachingType.Redis);
            }
        }
        else
        {
            var tokenValues = string.IsNullOrEmpty(oldTokens) ? accessToken : $"{oldTokens};{accessToken}";
            await _caching.SetAsync(key, tokenValues, TimeSpan.FromSeconds(DefaultJWTConfig.ExpiredSecond), onlyUseType: CachingType.Redis);
        }

        return accessToken;

    }  
    public string GenerateRefreshToken()
    {
        return Utility.RandomString(128);
    }
    
    public async Task<bool> CheckRefreshTokenAsync(string value, Guid ownerId, CancellationToken cancellationToken = default)
    {
        return await _authRepository.CheckRefreshTokenAsync(value, ownerId, cancellationToken);
    }
    
    public async Task RevokeAccessTokenAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        await _caching.SetAsync(
            AuthCacheKeys.GetRevokeAccessTokenKey(accessToken), 
            DateHelper.Now, 
            TimeSpan.FromSeconds(DefaultJWTConfig.ExpiredSecond));
    }
    
    public async Task<List<string>> RevokeAllAccessTokenAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var key = AuthCacheKeys.GetAccessTokenKey(userId);
        var oldTokens = await _caching.GetStringAsync(key);
        var tokens = oldTokens.Split(";");
        await Task.WhenAll(tokens.Select(token => RevokeAccessTokenAsync(token, cancellationToken)).Concat(new Task[] { _caching.DeleteAsync(key) }));
        return tokens.ToList();
    }
    
    public async Task<bool> IsNewLoginAddressAsync(RequestValue requestValue, CancellationToken cancellationToken = default)
    {
        return true;
    }

    public async Task<RequestValue> GetRequestValueAsync(CancellationToken cancellationToken = default)
    {
        var value = new RequestValue();
        var httpRequest = _httpContextAccessor.HttpContext?.Request;
        var header = httpRequest.Headers;
        var ua = header[HeaderNames.UserAgent].ToString();
        var c = Parser.GetDefault().Parse(ua);

        value.Ip = AuthUtility.TryGetIP(httpRequest);
        value.UA = ua;
        value.OS = c.OS.Family + (!string.IsNullOrEmpty(c.OS.Major) ? $" {c.OS.Major}" : "") + (!string.IsNullOrEmpty(c.OS.Minor) ? $".{c.OS.Minor}" : "");
        value.Browser = c.UA.Family + (!string.IsNullOrEmpty(c.UA.Major) ? $" {c.UA.Major}.{c.UA.Minor}" : "");
        value.Device = c.Device.Family;
        value.Origin = header[HeaderNames.Origin];
        value.Time = DateHelper.Now.ToString();
        value.IpInformation = await AuthUtility.GetIpInformationAsync(_provider, value.Ip);

        return value;
        
    }
    
}