namespace Whispr.Application.Features.V1.Users.Queries.GetUsers;

public static class GetUsersQueryErrors
{
    public static Error NoUsersFound => Error.Create(
        "GetUsersQuery.UsersNotFound",
        "No users found.",
        ErrorType.NotFound);
}