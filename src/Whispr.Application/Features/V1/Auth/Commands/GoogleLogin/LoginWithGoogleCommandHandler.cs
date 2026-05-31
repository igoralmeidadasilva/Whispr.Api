using System.Security.Claims;
using Whispr.Application.Core.Models.V1;
using Whispr.Domain.Core.Interfaces;
using Whispr.Domain.Core.Services;
using Whispr.Domain.Features.Entities.RefreshTokens;
using Whispr.Domain.Features.Entities.Users;
using Whispr.Domain.Features.Models;

namespace Whispr.Application.Features.V1.Auth.Commands.GoogleLogin;

internal sealed class LoginWithGoogleCommandHandler : ICommandHandler<LoginWithGoogleCommand, AuthTokenDto>
{
    private readonly IAuthTokenService _authTokenService;
    private readonly ITokenHasherService _tokenHasherService;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserPersistenceRepository _userPersistenceRepository;
    private readonly IRefreshTokenPersistenceRepository _refreshTokenPersistenceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LoginWithGoogleCommandHandler(
        IAuthTokenService authTokenService,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUserPersistenceRepository userPersistenceRepository,
        IRefreshTokenPersistenceRepository refreshTokenPersistenceRepository,
        IUnitOfWork unitOfWork,
        ITokenHasherService tokenHasherService)
    {
        _authTokenService = authTokenService;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userPersistenceRepository = userPersistenceRepository;
        _refreshTokenPersistenceRepository = refreshTokenPersistenceRepository;
        _unitOfWork = unitOfWork;
        _tokenHasherService = tokenHasherService;
    }

    public async Task<Result<AuthTokenDto>> Handle(LoginWithGoogleCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<Claim> claims = _authTokenService.GetClaimsFromAccessToken(request.IdToken);

        string? email = claims.FirstOrDefault(c => c.Type == "email")?.Value;
        string? name = claims.FirstOrDefault(c => c.Type == "name")?.Value;

        if (string.IsNullOrWhiteSpace(email))
        {
            return Result<AuthTokenDto>.Failure(LoginWithGoogleCommandErrors.EmailNotFound);
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<AuthTokenDto>.Failure(LoginWithGoogleCommandErrors.NameNotFound);
        }

        User user = await UpsertUserAsync(email!, name!, cancellationToken);

        TokenModel accessTokenModel = _authTokenService.GenerateAccessToken(user);
        TokenModel refreshTokenModel = _authTokenService.GenerateRefreshToken();

        RefreshToken refreshToken = new(
            user.Id,
            TokenHash.Create(_tokenHasherService.Hash(refreshTokenModel.Token)),
            refreshTokenModel.TokenExpirationAtUtc);

        _refreshTokenPersistenceRepository.Insert(refreshToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        AuthTokenDto authTokenDto = new()
        {
            AccessToken = accessTokenModel.Token,
            AccessTokenExpirationAtUtc = accessTokenModel.TokenExpirationAtUtc,
            RefreshToken = refreshTokenModel.Token,
            RefreshTokenExpirationAtUtc = refreshTokenModel.TokenExpirationAtUtc
        };

        return Result<AuthTokenDto>.Success(authTokenDto);
    }

    private async Task<User> UpsertUserAsync(string email, string name, CancellationToken cancellationToken = default)
    {
        User? user = await _userReadOnlyRepository.GetByEmailAsync(email, cancellationToken);

        if (user is not null)
        {
            return user;
        }

        var newUser = new User(name, email, Password.Empty);
        _userPersistenceRepository.Insert(newUser);

        return newUser;
    }
}