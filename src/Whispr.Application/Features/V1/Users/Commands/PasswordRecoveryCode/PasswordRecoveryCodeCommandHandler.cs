using System.Security.Cryptography;
using Whispr.Domain.Core.Interfaces;
using Whispr.Domain.Core.Services;
using Whispr.Domain.Features.Entities.PasswordResetTokens;
using Whispr.Domain.Features.Entities.Users;

namespace Whispr.Application.Features.V1.Users.Commands.PasswordRecoveryCode;

internal sealed class PasswordRecoveryCodeCommandHandler : ICommandHandler<PasswordRecoveryCodeCommand, Unit>
{
    private readonly IPasswordResetTokenPersistenceRepository _passwordResetTokenPersistenceRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly ITokenHasherService _tokenHasherService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly string EmailBody =
        """
        Olá, {Name}!

        Recebemos uma solicitação para redefinir a senha da sua conta no Whispr.

        Seu código de verificação é:

            {ResetCode}

        Este código é válido por 15 minutos. Não compartilhe este código com ninguém.

        Se você não solicitou a redefinição de senha, ignore este e-mail. Sua conta permanece segura.

        Atenciosamente,
        Equipe Whispr
        """;

    public PasswordRecoveryCodeCommandHandler(
        IPasswordResetTokenPersistenceRepository passwordResetTokenPersistenceRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        ITokenHasherService tokenHasherService,
        IUnitOfWork unitOfWork,
        IEmailService emailService)
    {
        _passwordResetTokenPersistenceRepository = passwordResetTokenPersistenceRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _tokenHasherService = tokenHasherService;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task<Result<Unit>> Handle(PasswordRecoveryCodeCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userReadOnlyRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
        {
            return Result<Unit>.Failure(PasswordRecoveryCodeCommandErrors.EmailNotFound);
        }

        var resetCode = RandomNumberGenerator.GetInt32(0, 10000).ToString("D4");

        string resetCodeHash = _tokenHasherService.Hash(resetCode);

        PasswordResetToken passwordResetToken = new(user.Id, TokenHash.Create(resetCodeHash));

        _passwordResetTokenPersistenceRepository.Insert(passwordResetToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var emailBody = EmailBody.Replace("{Name}", user.Name).Replace("{ResetCode}", resetCode);

        await _emailService.SendAsync(
            user.Email,
            user.Name,
            "Password recovery request",
            emailBody,
            cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}