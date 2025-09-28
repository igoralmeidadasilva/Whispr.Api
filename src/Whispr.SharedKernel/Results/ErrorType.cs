using System.Text.Json.Serialization;

namespace Whispr.SharedKernel.Results;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ErrorType
{
    Failure,
    Unexpected,
    BadRequest,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden
}