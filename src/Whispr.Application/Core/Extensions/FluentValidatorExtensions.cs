using FluentValidation;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Core.Extensions;

public static class FluentValidatorExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, Error error)
    {
        if (error is null)
            throw new ArgumentNullException(nameof(error), "The error is required");

        return rule.WithErrorCode(error.Code).WithMessage(error.Message);
    }
    
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string code, string message)
    {
        return rule.WithError(Error.Create(code, message));
    }
}