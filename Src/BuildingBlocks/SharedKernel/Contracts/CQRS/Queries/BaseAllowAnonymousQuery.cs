using Core.Security.Attributes;
using MediatR;
using static Core.Security.Enums.SecurityEnum;

namespace SharedKernel.Contracts
{
    [Authorization(new ActionExponent[] { ActionExponent.AllowAnonymous })]
    public class BaseAllowAnonymousQuery<TResponse> : BaseQuery<TResponse>
    {
    }
}
