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
            IEnumerable<string> validationResult = await ValidateAsync(request, cancellationToken);
            if (!validationResult.Any())
            {
                return await next(cancellationToken);
            }
            TResponse result = new()
            {
                IsSuccess = false,
                Error = Error.Create("Validation", string.Join("/n", validationResult), ErrorType.Validation)
            };
            return result;
        }
        return await next(cancellationToken);
    }

    private async Task<IList<string>> ValidateAsync(TRequest request, CancellationToken cancellationToken)
    {
        ValidationContext<TRequest> context = new(request);
        ValidationResult[] validationResults = await Task
            .WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        List<ValidationFailure> failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
        //ValidationResult validationResults = await _validator.ValidateAsync(context, cancellationToken);
        //var failures = validationResults.Errors.Where(f => f != null).ToList();
        if (failures.Count <= 0)
        {
            return [];
        }
        IList<string> errors = failures.Select(failure => failure.ErrorMessage).ToList();
        return errors;
    }
}