using Identity.Application.DTOs.Auth;
using SharedKernel.Contracts;

namespace Identity.Application.UseCase.VersionOne;

public interface IAuthUseCase
{
    Task<AuthResponse> RefreshTokenAsync(CancellationToken cancellationToken = default);

    Task<AuthResponse> SignInByAsync(SignInDto signInDto, CancellationToken cancellationToken = default);

    Task<AuthResponse> SignInByPhone(SignInByPhoneDto signInByPhoneDto, CancellationToken cancellationToken = default);

    Task<bool> SignOutAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<bool> SignOutAllDeviceAsync(Guid userId, CancellationToken cancellationToken = default);
    
    Task<bool> SignUpAsync(SignUpDto signUpDto, CancellationToken cancellationToken = default);

    Task<IPagedList<SignInHistoryDto>> GetSignInHistoryPaging(PagingRequest pagingRequest, CancellationToken cancellationToken = default);
} 