using Microsoft.Extensions.Logging;
using Whispr.Application.Core.Interfaces;
using Whispr.Application.Core.Models.V1;
using Whispr.Domain.Features.Entities.User;
using Whispr.SharedKernel.Pagination;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Features.V1.Users.Queries.GetUsers;

internal sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, PagedList<UserDto>>
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly ILogger<GetUsersQueryHandler> _logger;

    public GetUsersQueryHandler(IUserReadOnlyRepository userReadOnlyRepository, ILogger<GetUsersQueryHandler> logger)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _logger = logger;
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
 
        List<UserDto> usersDto = users.Items.Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.Name!,
            Email = user.Email!
        }).ToList();

        PagedList<UserDto> page = new PagedList<UserDto>(
            usersDto,
            users.TotalCount,
            users.PageNumber,
            users.PageSize);

        return Result<PagedList<UserDto>>.Success(page);
    }
}