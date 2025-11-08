using Microsoft.AspNetCore.Mvc;
using Whispr.SharedKernel.Results;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Whispr.Presentation.Api.Core.Extensions;

public static class ResultExtensions
{
    public static IResult Match<TValue>(
        this Result<TValue> result,
        Func<IResult> onSuccess,
        Func<object, IResult> onFailure,
        bool enableValidationAutoResult = true)
    {
        if (!result.IsFailure)
        {
            return onSuccess();
        }

        if (!enableValidationAutoResult)
        {
            return onFailure(result.Error.ToProblemDetails());
        }

        return result.Error.Type switch
        {
            ErrorType.Validation => Results.BadRequest(result.Error.ToProblemDetails()),
            ErrorType.Conflict => Results.Conflict(result.Error.ToProblemDetails()),
            _ => onFailure(result.Error.ToProblemDetails())
        };
    }

    public static IResult Match<TValue>(
        this Result<TValue> result,
        Func<object, IResult> onSuccess,
        Func<object, IResult> onFailure,
        bool enableValidationAutoResult = true)
    {
        if (!result.IsFailure)
        {
            return onSuccess(result.Value!);
        }

        if (!enableValidationAutoResult)
        {
            return onFailure(result.Error.ToProblemDetails());
        }

        return result.Error.Type switch
        {
            ErrorType.Validation => Results.BadRequest(result.Error.ToProblemDetails()),
            ErrorType.Conflict => Results.Conflict(result.Error.ToProblemDetails()),
            _ => onFailure(result.Error.ToProblemDetails())
        };
    }

    public static ProblemDetails ToProblemDetails(this Error error, string? instance = null)
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