using Identity.Application.Constants;
using Identity.Application.DTOs.Auth;
using Identity.Application.Properties;
using Identity.Application.Repositories.Interfaces;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Net.Http.Headers;
using SharedKernel.Auth;
using SharedKernel.Contracts;
using SharedKernel.Core;
using SharedKernel.Domain;
using SharedKernel.Libraries;
using SharedKernel.Runtime.Exceptions;
using UAParser;

namespace Identity.Application.UseCase.VersionOne;

public class AuthUseCase : IAuthUseCase
{
    private readonly IAuthRepository _authRepository;
    private readonly IAuthService _authService;
    private readonly IStringLocalizer<Resources> _localizer;
    private readonly ICurrentUser _currentUser;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceProvider _provider;
    public AuthUseCase(
        IAuthRepository authRepository, 
        IAuthService authService, 
        IHttpContextAccessor httpContextAccessor,
        IServiceProvider provider)
    {
        _authRepository = authRepository;
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
        _provider = provider;
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken = default)
    {
        var isValid = await _authService.CheckRefreshTokenAsync(refreshTokenDto.RefreshToken, refreshTokenDto.UserId, cancellationToken);
        if (!isValid)
        {
            throw new BadRequestException("The refresh-token is invalid or expired!");
        }
        
        var tokenUser = await _authRepository.GetTokenUserByOwnerIdAsync(refreshTokenDto.UserId, cancellationToken);
        if (tokenUser == null)
        {
            throw new BadRequestException("Currently, this account does not exist.");
        }
        
        var currentAccessToken = await _authService.GenerateAccessTokenAsync(tokenUser, cancellationToken);
        var refreshToken = new RefreshToken
        {
            RefreshTokenValue = refreshTokenDto.RefreshToken,
            CurrentAccessToken= currentAccessToken,
            OwnerId= refreshTokenDto.UserId,
            ExpiredDate = DateHelper.Now.AddSeconds(AuthConstant.REFRESH_TOKEN_TIME)
        };

        await _authRepository.CreateOrUpdateRefreshTokenAsync(refreshToken, cancellationToken);
        await _authRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new AuthResponse { AccessToken = currentAccessToken, RefreshToken = refreshTokenDto.RefreshToken };
    }
    
    public async Task<AuthResponse> SignInByPhone(SignInByPhoneDto signInByPhoneDto, CancellationToken cancellationToken = default)
    {
        var tokenUser = await _authRepository.GetTokenUserByIdentityAsync(signInByPhoneDto.Phone, signInByPhoneDto.Password, cancellationToken);
        if (tokenUser == null)
        {
            throw new BadRequestException(_localizer["auth_sign_in_info_incorrect"].Value);
        }

        var isNewLoginAddress = await _authService.IsNewLoginAddressAsync(cancellationToken);
        if (!isNewLoginAddress) // is not new login address
        {
            var accessTokenValue = await _authService.GenerateAccessTokenAsync(tokenUser, cancellationToken);
            var refreshTokenValue = _authService.GenerateRefreshToken();
            
            // Save refresh token
            var refreshToken = new RefreshToken
            {
                RefreshTokenValue = refreshTokenValue,
                CurrentAccessToken = accessTokenValue,
                OwnerId = tokenUser.Id,
                ExpiredDate = DateHelper.Now.AddSeconds(AuthConstant.REFRESH_TOKEN_TIME),
                CreatedBy = tokenUser.Id,
            };
        
            await _authRepository.CreateOrUpdateRefreshTokenAsync(refreshToken, cancellationToken);
            await _authRepository.UnitOfWork.CommitAsync(cancellationToken);

            return new AuthResponse { AccessToken = accessTokenValue, RefreshToken = refreshTokenValue };
        }
        
        // Create otp, send otp for user by phone - OS-{0} là mã xác nhận Open School của bạn
        var otp = new OTP()
        {
            Otp = _authService.GenerateOtp(),
            IsUsed = false, // OTP code has not been used - Mã OTP chưa được sử dụng
            ExpiredDate = DateHelper.Now.AddMinutes(AuthConstant.MAX_TIME_OTP), // OTP expiration time - Thời gian hết hạn OTP
            ProvidedDate = DateHelper.Now, // Time to generate OTP code - Thời điểm tạo mã OTP
            Type = OtpType.Verify
        };
        await _authRepository.CreateOtpAsync(otp, cancellationToken);
        await _authRepository.UnitOfWork.CommitAsync(cancellationToken);
        
        
        // EventBus: đẩy message lên rabbitMQ (save change signInHistory and send notification đi các máy đang đăng nhập)
        
        return new AuthResponse { IsVerifyCode = true };
    }
    

    public async Task<bool> SignOutAsync(CancellationToken cancellationToken = default)
    {
        // Revoke access token
        await _authService.RevokeAccessTokenAsync(_currentUser.Context.AccessToken, cancellationToken);

        // Xóa refresh token
        await _authRepository.RemoveRefreshTokenAsync(cancellationToken);
        

        if (CoreSettings.IsSingleDevice)
        {
          
        }
        
        return true;
    }

    public async Task<bool> SignOutAllDeviceAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        // Revoke access token
        var accessTokens = await _authService.RevokeAllAccessTokenAsync(userId, cancellationToken);

        // Xóa refresh token
        await _authRepository.RemoveRefreshTokenAsync(accessTokens, cancellationToken);
        
        if (CoreSettings.IsSingleDevice)
        {
          
        }
        
        return true;
    }

    public async Task<RequestValue> GetRequestInformationAsync(CancellationToken cancellationToken = default)
    {
        var value = new RequestValue();
        var httpRequest = _currentUser.Context.HttpContext.Request;
        var header = httpRequest.Headers;
        var ua = header[HeaderNames.UserAgent].ToString();
        var c = Parser.GetDefault().Parse(ua);

        value.Ip = AuthUtility.TryGetIP(httpRequest);
        value.UA = ua;
        value.OS = c.OS.Family + (!string.IsNullOrEmpty(c.OS.Major) ? $" {c.OS.Major}" : "") + (!string.IsNullOrEmpty(c.OS.Minor) ? $".{c.OS.Minor}" : "");
        value.Browser = c.UA.Family + (!string.IsNullOrEmpty(c.UA.Major) ? $" {c.UA.Major}.{c.UA.Minor}" : "");
        value.Device = c.Device.Family;
        value.Origin = header[HeaderNames.Origin].ToString();
        value.Time = DateHelper.Now.ToString();
        value.IpInformation = await AuthUtility.GetIpInformationAsync(_provider, value.Ip);

        return value;
    }

    public async Task<IPagedList<SignInHistoryDto>> GetSignInHistoryPaging(PagingRequest pagingRequest, CancellationToken cancellationToken = default)
    {
        return await _authRepository.GetSignInHistoryPagingAsync(pagingRequest, cancellationToken);
    }
}