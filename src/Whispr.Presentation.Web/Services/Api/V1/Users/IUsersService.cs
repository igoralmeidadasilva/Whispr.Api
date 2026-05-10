using Whispr.Presentation.Web.Core.Dtos;
using Whispr.Presentation.Web.Core.Http;
using Whispr.Presentation.Web.Core.Models;
using Whispr.Presentation.Web.Services.Api.V1.Users.Requests;

namespace Whispr.Presentation.Web.Services.Api.V1.Users;

public interface IUsersService
{
    Task<ApiResponse<PagedModel<UserDto>>> GetAllAsync(GetUsersRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<UserDto>> GetByIdAsync(GetUserByIdRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<NoContent>> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<NoContent>> UpdateAsync(UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<NoContent>> DeleteAsync(DeleteUserRequest request, CancellationToken cancellationToken = default);
}