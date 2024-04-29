using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Caching;
using Identity.Application.Constants;
using Identity.Application.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using SharedKernel.Auth;
using SharedKernel.Contracts;
using SharedKernel.Core;
using SharedKernel.Libraries;
using SharedKernel.Runtime.Exceptions;
using UAParser;
using Utility = SharedKernel.Libraries.Utility;

namespace Identity.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;
    private readonly ISequenceCaching _caching;
    private readonly ICurrentUser _currentUser;
    public AuthService(
        IAuthRepository authRepository,
        IConfiguration configuration,
        ISequenceCaching caching,
        ICurrentUser currentUser)
    {
        _authRepository = authRepository;
        _configuration = configuration;
        _caching = caching;
        _currentUser = currentUser;
    }
    
    public bool CheckPermission(ActionExponent exponent)
    {
        return CheckPermission(new ActionExponent[] { exponent });
    }

    public bool CheckPermission(ActionExponent[] exponents)
    {
        var length = exponents.Length;
        for (int i = 0; i < length; i++)
        {
            var action = AuthUtility.FromExponentToPermission((int)exponents[i]);
            if (!AuthUtility.ComparePermissionAsString(_currentUser.Context.Permission, action))
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
        claims.Add(new Claim(ClaimConstant.CREATE_AT, token.CreatedDate.ToString("MM/dd/yyyy HH:mm:ss")));
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
        
        var key = BaseCacheKeys.GetAccessTokenKey(token.Id);
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
            BaseCacheKeys.GetRevokeAccessTokenKey(accessToken), 
            DateHelper.Now, 
            TimeSpan.FromSeconds(DefaultJWTConfig.ExpiredSecond));
    }
    
    public async Task<List<string>> RevokeAllAccessTokenAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var key = BaseCacheKeys.GetAccessTokenKey(userId);
        var oldTokens = await _caching.GetStringAsync(key);
        var tokens = oldTokens.Split(";");
        await Task.WhenAll(tokens.Select(token => RevokeAccessTokenAsync(token, cancellationToken)).Concat(new Task[] { _caching.DeleteAsync(key) }));
        return tokens.ToList();
    }
    
    public async Task<bool> IsNewLoginAddressAsync(CancellationToken cancellationToken = default)
    {
        var request = _currentUser.Context.HttpContext?.Request;
        if (request == null) throw new BadRequestException("");
        
        var ua = request.Headers[HeaderNames.UserAgent].ToString(); // Thông tin trình duyệt và hệ điều hành
        var c = Parser.GetDefault().Parse(ua); 
        var device = c.Device.Family;
        var ip = AuthUtility.TryGetIP(request);
        var browser = c.UA.Family + (!string.IsNullOrEmpty(c.UA.Major) ? $" {c.UA.Major}.{c.UA.Minor}" : "");
        var os = c.OS.Family + (!string.IsNullOrEmpty(c.OS.Major) ? $" {c.OS.Major}" : "") + (!string.IsNullOrEmpty(c.OS.Minor) ? $".{c.OS.Minor}" : "");

        return await _authRepository.CheckSignInHistoryAsync(ip, ua, device, browser, os, cancellationToken);
    }

    public string GenerateOtp()
    {
        return Utility.RandomNumber(AuthConstant.OTP_LENGTH);
    }
}