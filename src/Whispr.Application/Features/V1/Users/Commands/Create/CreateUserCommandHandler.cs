using Whispr.Domain.Core.Interfaces;
using Whispr.Domain.Core.Services;
using Whispr.Domain.Features.Entities.Users;

namespace Whispr.Application.Features.V1.Users.Commands.Create;

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Unit>
{
    private readonly IUserPersistenceRepository _userPersistenceRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IPasswordHasherService _passwordHasherService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(
        IUserPersistenceRepository userPersistenceRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IPasswordHasherService passwordHasherService,
        IUnitOfWork unitOfWork)
    {
        _userPersistenceRepository = userPersistenceRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _passwordHasherService = passwordHasherService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        User? findByEmail = await _userReadOnlyRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (findByEmail is not null)
        {
            return Result<Unit>.Failure(CreateUserCommandErrors.EmailAlreadyExists);
        }
        
        User? findByName = await _userReadOnlyRepository.GetByNameAsync(request.Username, cancellationToken);
        if (findByName is not null)
        {
            return Result<Unit>.Failure(CreateUserCommandErrors.NameAlreadyExists);
        }

        var passwordHash = Password.Create(_passwordHasherService, request.Password);
        
        User newUser = new(request.Username, request.Email, passwordHash);

        _userPersistenceRepository.Insert(newUser);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}