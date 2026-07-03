using Whispr.Application.Core.Mappings;
using Whispr.Application.Core.Models.V1;
using Whispr.Domain.Features.Entities.Users;

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

        UserDto userDto = UserMappings.ToUserDto(user);

        return Result<UserDto>.Success(userDto);
    }
}