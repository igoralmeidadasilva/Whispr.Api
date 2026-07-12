using Whispr.Domain.Core.Services;
using Whispr.Domain.Features.Entities.RefreshTokens;

namespace Whispr.Application.Features.V1.Auth.Commands.Logout;

internal sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand, Unit>
{
    private readonly IRefreshTokenPersistenceRepository _refreshTokenPersistenceRepository;
    private readonly IRefreshTokenReadOnlyRepository _refreshTokenReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenHasherService _tokenHasherService;

    public LogoutCommandHandler(
        IRefreshTokenPersistenceRepository refreshTokenPersistenceRepository,
        IRefreshTokenReadOnlyRepository refreshTokenReadOnlyRepository,
        IUnitOfWork unitOfWork,
        ITokenHasherService tokenHasherService)
    {
        _refreshTokenPersistenceRepository = refreshTokenPersistenceRepository;
        _refreshTokenReadOnlyRepository = refreshTokenReadOnlyRepository;
        _unitOfWork = unitOfWork;
        _tokenHasherService = tokenHasherService;
    }

    public async Task<Result<Unit>> Handle(LogoutCommand request, CancellationToken cancellationToken = default)
    {
        var refreshToken = await _refreshTokenReadOnlyRepository.GetByTokenHashAsync(
            _tokenHasherService.Hash(request.RefreshToken),
            cancellationToken);
        if (refreshToken is null)
        {
            return Result<Unit>.Failure(LogoutCommandErrors.RefreshTokenNotFound);
        }

        refreshToken.Revoke();
        _refreshTokenPersistenceRepository.Update(refreshToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}
