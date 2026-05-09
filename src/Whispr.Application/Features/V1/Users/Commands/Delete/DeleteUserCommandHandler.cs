using Whispr.Domain.Core.Interfaces;
using Whispr.Domain.Features.Entities.User;

namespace Whispr.Application.Features.V1.Users.Commands.Delete;

internal sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, Unit>
{
    private readonly IUserPersistenceRepository _userPersistenceRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(
        IUserPersistenceRepository userPersistenceRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _userPersistenceRepository = userPersistenceRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userReadOnlyRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result<Unit>.Failure(DeleteUserCommandErrors.UserIdNotFound);
        }

        _userPersistenceRepository.Delete(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}