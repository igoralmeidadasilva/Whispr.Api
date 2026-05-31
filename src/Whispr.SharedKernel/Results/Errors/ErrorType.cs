using System.Text.Json.Serialization;

namespace Whispr.SharedKernel.Results.Errors;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ErrorType
{
    None,
    Failure,
    BadRequest,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden,
    Validation
}