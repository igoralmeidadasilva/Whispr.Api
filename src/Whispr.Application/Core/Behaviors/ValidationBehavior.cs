using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Whispr.SharedKernel.Results;
using Whispr.SharedKernel.Results.Factories;

namespace Whispr.Application.Core.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : IBaseResult
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly IResultFactory<TResponse> _resultFactory;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        IResultFactory<TResponse> resultFactory)
    {
        _validators = validators;
        _resultFactory = resultFactory;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next(cancellationToken);
        }

        var validationError = await ValidateAsync(request, cancellationToken);

        if (validationError is not null)
        {
            var result = _resultFactory.Failure(validationError);
            return result;
        }

        return await next(cancellationToken);
    }

    private async Task<ValidationError?> ValidateAsync(TRequest request, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var results = await Task
            .WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var errors = results
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .GroupBy(f => f.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(f => f.ErrorMessage).ToArray());

        if (errors.Count == 0)
        {
            return null;
        }

        return ValidationError.Create(errors, "Validation Failure", "One or more validation errors were found.");
    }
}