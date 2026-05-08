using Whispr.Presentation.Web.Core.Dtos;
using Whispr.Presentation.Web.Core.Http;
using Whispr.Presentation.Web.Services.Api.V1.Users.Requests;

namespace Whispr.Presentation.Web.Services.Api.V1.Users;

public interface IUsersService
{
    Task<ApiResponse<IEnumerable<UserDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<UserDto>> GetByIdAsync(CancellationToken cancellationToken = default);
    Task<NoContent?> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<NoContent>> UpdateAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<NoContent>> DeleteAsync(CancellationToken cancellationToken = default);
}