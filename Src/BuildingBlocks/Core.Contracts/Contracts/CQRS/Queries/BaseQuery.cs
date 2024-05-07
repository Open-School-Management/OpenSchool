using Core.Security.Attributes;
using MediatR;
using static Core.Security.Enums.SecurityEnum;

namespace SharedKernel.Contracts
{
    [AuthorizationRequest]
    public abstract class BaseQuery<TResponse> : IRequest<TResponse>
    {
    }
}
