using System.Text.Json.Serialization;

namespace Whispr.SharedKernel.Results;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ErrorType
{
    None,
    Failure,
    Unexpected,
    BadRequest,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden
}