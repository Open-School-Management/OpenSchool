using Core.Security.Attributes;
using MediatR;
using static Core.Security.Enums.SecurityEnum;

namespace SharedKernel.Contracts;

[AuthorizationRequest(new ActionExponent[] { ActionExponent.AllowAnonymous })]
public class BaseAllowAnonymousCommand<TResponse> : BaseCommand<TResponse>
{
}

public class BaseAllowAnonymousCommand : BaseAllowAnonymousCommand<Unit>
{
}