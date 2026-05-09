using Whispr.Application.Core.Models.V1;
using Whispr.Domain.Features.Entities.User;

namespace Whispr.Application.Features.V1.Users.Queries.GetById;

internal sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;

    public GetUserByIdQueryHandler(IUserReadOnlyRepository userReadOnlyRepository)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        User? user = await _userReadOnlyRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result<UserDto>.Failure(GetUserByIdQueryErrors.UserNotFound);
        }

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Name,
            Email = user.Email
        };

        return Result<UserDto>.Success(userDto);
    }
}