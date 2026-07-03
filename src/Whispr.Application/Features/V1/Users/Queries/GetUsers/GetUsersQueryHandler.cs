using Whispr.Application.Core.Mappings;
using Whispr.Application.Core.Models.V1;
using Whispr.Domain.Features.Entities.Users;
using Whispr.SharedKernel.Pagination;

namespace Whispr.Application.Features.V1.Users.Queries.GetUsers;

internal sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, PagedList<UserDto>>
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;

    public GetUsersQueryHandler(IUserReadOnlyRepository userReadOnlyRepository)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task<Result<PagedList<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        PagedList<User> users = await _userReadOnlyRepository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        if (!users.Items.Any())
        {
            return Result<PagedList<UserDto>>.Success(PagedList<UserDto>.Empty());
        }

        List<UserDto> usersDto = users.Items
            .Select(UserMappings.ToUserDto)
            .ToList();

        PagedList<UserDto> page = new(
            usersDto,
            users.TotalCount,
            users.PageNumber,
            users.PageSize);

        return Result<PagedList<UserDto>>.Success(page);
    }
}