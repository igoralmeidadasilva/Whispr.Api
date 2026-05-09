using Whispr.Domain.Core.Interfaces;
using Whispr.Domain.Features.Entities.RefreshToken;

namespace Whispr.Application.Features.V1.Auth.Commands.Logout;

internal sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand, Unit>
{
    private readonly IRefreshTokenPersistenceRepository _refreshTokenPersistenceRepository;
    private readonly IRefreshTokenReadOnlyRepository _refreshTokenReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(
        IRefreshTokenPersistenceRepository refreshTokenPersistenceRepository,
        IRefreshTokenReadOnlyRepository refreshTokenReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _refreshTokenPersistenceRepository = refreshTokenPersistenceRepository;
        _refreshTokenReadOnlyRepository = refreshTokenReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(LogoutCommand request, CancellationToken cancellationToken = default)
    {
        var refreshToken = await _refreshTokenReadOnlyRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);
        if (refreshToken is null)
        {
            return Result<Unit>.Failure(LogoutCommandErrors.RefreshTokenNotFound);
        }

        _refreshTokenPersistenceRepository.Delete(refreshToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}
