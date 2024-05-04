using Identity.Application.DTOs.Auth;
using Identity.Application.UseCase.VersionOne;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Contracts;

namespace Identity.Api.Controllers.VersionOne;

[ApiVersion("1.0")]
public class AuthController : BaseController
{
    private readonly IAuthUseCase _authUseCase;
    public AuthController(IAuthUseCase authUseCase)
    {
        _authUseCase = authUseCase;
    }
    
    [AllowAnonymous]
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignInByPhoneAsync(SignInByPhoneDto request, CancellationToken cancellationToken = default)
    {
        var authResponse = await _authUseCase.SignInByPhone(request, cancellationToken);
        return Ok(new ApiSimpleResult(authResponse));
    }
    
}