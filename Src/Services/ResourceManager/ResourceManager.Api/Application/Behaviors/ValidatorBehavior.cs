using FluentValidation;
using MediatR;
using MessageBroker.Abstractions.Extensions;

namespace ResourceManager.Api.Application.Behaviors;

public class ValidatorBehavior<TRequest, TResponse>(
    ILogger<ValidatorBehavior<TRequest, TResponse>> logger,
    IEnumerable<IValidator<TRequest>> validators
    ) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var typeName = request.GetGenericTypeName();

        logger.LogInformation("Validating command {CommandType}", typeName);
        
        var failures = validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (failures.Any())
        {
            logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);

            throw new ValidationException("Validation exception", failures);
        }

        
        return await next();
    }
}