using Microsoft.AspNetCore.Mvc;
using Whispr.SharedKernel.Results;

namespace Whispr.Presentation.Api.Core.Extensions;

public static class ResultExtensions
{
    public static IResult Match<TValue>(
        this Result<TValue> result,
        Func<object, IResult> successFunc)
    {
        if (result.IsFailure)
        {
            return Results.Problem(result.Error.ToProblemDetails());
        }
        return successFunc(result.Value!);
    }

    public static IResult Match<TValue>(
        this Result<TValue> result,
        Func<IResult> successFunc,
        string? instance = null)
    {
        if (result.IsFailure)
        {
            return Results.Problem(result.Error.ToProblemDetails(instance));
        }
        return successFunc();
    }

    public static ProblemDetails ToProblemDetails(this Error error, string? instance = null)
    {
        if (error.Type == ErrorType.Validation)
        {
            if (error is ValidationError validationError)
            {
                return ToValidationProblemDetailsCore(validationError, instance);
            }
        }
        return ToProblemDetailsCore(error, instance);
    }

    private static ValidationProblemDetails ToValidationProblemDetailsCore(this ValidationError error, string? instance = null)
    {
        int statusCode = error.Type.ToStatusCode();
        return new ValidationProblemDetails
        {
            Type = $"https://httpstatuses.com/{statusCode}",
            Title = error.Code,
            Status = statusCode,
            Detail = error.Message,
            Instance = instance,
            Errors = error.Failures,
        };
    }

    private static ProblemDetails ToProblemDetailsCore(this Error error, string? instance = null)
    {
        int statusCode = error.Type.ToStatusCode();
        return new ProblemDetails()
        {
            Type = $"https://httpstatuses.com/{statusCode}",
            Title = error.Code,
            Status = statusCode,
            Detail = error.Message,
            Instance = instance,
        };
    }

    public static int ToStatusCode(this ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Failure => 400,
            ErrorType.BadRequest => 400,
            ErrorType.Conflict => 409,
            ErrorType.NotFound => 404,
            ErrorType.Unauthorized => 401,
            ErrorType.Forbidden => 403,
            ErrorType.Validation => 400,
            _ => 500
        };
    }
}