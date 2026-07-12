using Whispr.Domain.Core.Services;
using Whispr.Domain.Features.Entities.PasswordResetTokens;
using Whispr.Domain.Features.Entities.Users;

namespace Whispr.Application.Features.V1.Users.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, Unit>
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserPersistenceRepository _userPersistenceRepository;
    private readonly IPasswordResetTokenReadOnlyRepository _passwordResetTokenReadOnlyRepository;
    private readonly IPasswordResetTokenPersistenceRepository _passwordResetTokenPersistenceRepository;
    private readonly ITokenHasherService _tokenHasherService;
    private readonly IPasswordHasherService _passwordHasherService;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordCommandHandler(
        IUserReadOnlyRepository userReadOnlyRepository,
        IUserPersistenceRepository userPersistenceRepository,
        IPasswordResetTokenReadOnlyRepository passwordResetTokenReadOnlyRepository,
        IPasswordResetTokenPersistenceRepository passwordResetTokenPersistenceRepository,
        ITokenHasherService tokenHasherService,
        IPasswordHasherService passwordHasherService,
        IUnitOfWork unitOfWork)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _userPersistenceRepository = userPersistenceRepository;
        _passwordResetTokenReadOnlyRepository = passwordResetTokenReadOnlyRepository;
        _passwordResetTokenPersistenceRepository = passwordResetTokenPersistenceRepository;
        _tokenHasherService = tokenHasherService;
        _passwordHasherService = passwordHasherService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userReadOnlyRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
        {
            return Result<Unit>.Failure(ChangePasswordCommandErrors.PasswordResetTokenInvalid);
        }

        PasswordResetToken? passwordResetToken = await _passwordResetTokenReadOnlyRepository.GetLatestActiveByUserIdAsync(
            user.Id,
            cancellationToken);
        if (passwordResetToken is null)
        {
            return Result<Unit>.Failure(ChangePasswordCommandErrors.PasswordResetTokenInvalid);
        }

        if (!passwordResetToken.IsValid)
        {
            return Result<Unit>.Failure(ChangePasswordCommandErrors.PasswordResetTokenInvalid);
        }

        if (!_tokenHasherService.Verify(request.RecoveryCode, passwordResetToken.TokenHash.Value))
        {
            passwordResetToken.IncreaseAttempt();

            _passwordResetTokenPersistenceRepository.Update(passwordResetToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Unit>.Failure(ChangePasswordCommandErrors.PasswordResetTokenInvalid);
        }

        passwordResetToken.MarkAsUsed();
        Password passwordHash = Password.Create(_passwordHasherService, request.NewPassword);
        user.ChangePassword(passwordHash);

        _userPersistenceRepository.Update(user);
        _passwordResetTokenPersistenceRepository.Update(passwordResetToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}