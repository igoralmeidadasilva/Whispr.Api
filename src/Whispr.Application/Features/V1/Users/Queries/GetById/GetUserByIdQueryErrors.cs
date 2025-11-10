using Whispr.SharedKernel.Results;

namespace Whispr.Application.Features.V1.Users.Queries.GetById;

public static class GetUserByIdQueryErrors
{
    public static Error UserNotFound => Error.Create(
        "GetUsersQuery.UserNotFound",
        "No users were found with this Id: ",
        ErrorType.NotFound);
}