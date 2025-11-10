using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Core.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : IBaseResult, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators != null)
        {
            await ValidateAsync(request, cancellationToken);
        }
        return await next(cancellationToken);
    }

    private async Task ValidateAsync(TRequest request, CancellationToken cancellationToken)
    {
        ValidationContext<TRequest> context = new(request);
        ValidationResult[] validationResults = await Task
            .WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        List<ValidationFailure> failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
        if (failures.Count <= 0)
        {
            return;
        }
        throw new ValidationException(failures);
    }
}