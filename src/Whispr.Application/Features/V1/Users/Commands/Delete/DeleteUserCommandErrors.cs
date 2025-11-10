using Whispr.SharedKernel.Results;

namespace Whispr.Application.Features.V1.Users.Commands.Delete;

public static class DeleteUserCommandErrors
{
    public static Error UserIdNotFound => Error.Create(
       "DeleteUserCommand.UserId.NotFound",
       "User id cannot be found.",
       ErrorType.NotFound);

    public static Error IdentityFailure(string message) => Error.Create(
        "DeleteUserCommand.Identity.Failure",
        message,
        ErrorType.Failure);
}