using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Core.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : IResult, new()
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
            IEnumerable<Error> validationResult = await ValidateAsync(request, cancellationToken);
            if (validationResult.Any())
            {
                TResponse result = new();
                foreach (var error in validationResult)
                {
                    result.Errors.Add(error);
                }
                return result;
            }
        }
        return await next(cancellationToken);
    }

    private async Task<IEnumerable<Error>> ValidateAsync(TRequest request, CancellationToken cancellationToken)
    {
        ValidationContext<TRequest> context = new(request);
        ValidationResult[] validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        IEnumerable<ValidationFailure> failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null);

        if (failures.Any())
        {
            IEnumerable<Error> errors = failures.Select(failure => Error.Create(failure.ErrorCode, failure.ErrorMessage));
            return errors;
        }
        return [];
    }
}