using Whispr.SharedKernel.Results.Errors;

namespace Whispr.Application.Features.V1.Users.Queries.GetById;

public static class GetUserByIdQueryErrors
{
    public static Error UserIdIsRequired => Error.Create(
        "GetUserByIdQuery.UserId.IsRequired",
        "UserId is required.",
        ErrorType.Validation);

    public static Error UserNotFound => Error.Create(
        "GetUserByIdQuery.UserNotFound",
        "No user was found with this Id: ",
        ErrorType.NotFound);
}