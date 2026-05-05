using System.Net;

namespace Whispr.Presentation.Web.Core.Http;

public sealed class ApiResponse<T>
{
    public T? Value { get; set; }
    public ProblemDetails? ProblemDetails { get; set; }
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;
    public HttpStatusCode StatusCode { get; set; }

    private ApiResponse(T? value, ProblemDetails? problemDetails, bool isSuccess, HttpStatusCode statusCode)
    {
        IsSuccess = isSuccess; 
        Value = value;
        ProblemDetails = problemDetails;
        StatusCode = statusCode;
    }

    public static ApiResponse<T> Success(T? value, HttpStatusCode statusCode)
    {
        return new ApiResponse<T>(value, null, true, statusCode);
    }

    public static ApiResponse<T> Failure(ProblemDetails? problemDetails, HttpStatusCode statusCode)
    {
        return new ApiResponse<T>(default, problemDetails, false, statusCode);
    }
}