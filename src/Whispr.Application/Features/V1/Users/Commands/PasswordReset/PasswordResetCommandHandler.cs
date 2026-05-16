using Whispr.Domain.Core.Interfaces;
using Whispr.Domain.Core.Services;
using Whispr.Domain.Features.Entities.PasswordResetTokens;
using Whispr.Domain.Features.Entities.Users;

namespace Whispr.Application.Features.V1.Users.Commands.PasswordReset;

internal sealed class PasswordResetCommandHandler : ICommandHandler<PasswordResetCommand, Unit>
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserPersistenceRepository _userPersistenceRepository;
    private readonly IPasswordResetTokenReadOnlyRepository _passwordResetTokenReadOnlyRepository;
    private readonly ITokenHasherService _tokenHasherService;
    private readonly IPasswordHasherService _passwordHasherService;
    private readonly IUnitOfWork _unitOfWork;

    public PasswordResetCommandHandler(
        IUserReadOnlyRepository userReadOnlyRepository,
        IPasswordResetTokenReadOnlyRepository passwordResetTokenReadOnlyRepository,
        ITokenHasherService tokenHasherService,
        IPasswordHasherService passwordHasherService,
        IUserPersistenceRepository userPersistenceRepository,
        IUnitOfWork unitOfWork)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _passwordResetTokenReadOnlyRepository = passwordResetTokenReadOnlyRepository;
        _tokenHasherService = tokenHasherService;
        _passwordHasherService = passwordHasherService;
        _userPersistenceRepository = userPersistenceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(PasswordResetCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userReadOnlyRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
        {
            return Result<Unit>.Failure(PasswordResetCommandErrors.EmailNotFound);
        }

        string recoveryCodeHash = _tokenHasherService.Hash(request.RecoveryCode);

        PasswordResetToken? passwordResetToken = await _passwordResetTokenReadOnlyRepository.GetByTokenHashAsync(
            recoveryCodeHash,
            cancellationToken);
        if (passwordResetToken is null)
        {
            return Result<Unit>.Failure(PasswordResetCommandErrors.PasswordResetTokenNotFound);
        }

        if (recoveryCodeHash != passwordResetToken.TokenHash.Value)
        {
            return Result<Unit>.Failure(PasswordResetCommandErrors.PasswordResetTokenInvalid);
        }

        Password passwordHash = Password.Create(_passwordHasherService, request.NewPassword);
        user.ChangePassword(passwordHash);

        _userPersistenceRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}