using MediatR;
using Whispr.Application.Core.Abstractions;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Features.V1.Users.Commands.Delete;

public sealed record DeleteUserCommand : ICommand<Result<Unit>>
{
    public required Guid UserId { get; init; }
}