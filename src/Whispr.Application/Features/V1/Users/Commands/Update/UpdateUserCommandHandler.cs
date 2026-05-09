using Whispr.Domain.Core.Interfaces;
using Whispr.Domain.Features.Entities.User;

namespace Whispr.Application.Features.V1.Users.Commands.Update;

internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Unit>
{
    private readonly IUserPersistenceRepository _userPersistenceRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(IUserPersistenceRepository userPersistenceRepository, IUserReadOnlyRepository userReadOnlyRepository, IUnitOfWork unitOfWork)
    {
        _userPersistenceRepository = userPersistenceRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userReadOnlyRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result<Unit>.Failure(UpdateUserCommandErrors.UserIdNotFound);
        }

        if (request.Email != user.Email)
        {
            User? findByEmail = await _userReadOnlyRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (findByEmail != null)
            {
                return Result<Unit>.Failure(UpdateUserCommandErrors.EmailAlreadyExists);
            }   
        }

        if (request.UserName != user.Name)
        {
            User? findByName = await _userReadOnlyRepository.GetByNameAsync(request.UserName, cancellationToken);
            if (findByName != null)
            {
                return Result<Unit>.Failure(UpdateUserCommandErrors.UserNameAlreadyExists);
            }
        }

        user.Update(request.UserName, request.Email);

        _userPersistenceRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}