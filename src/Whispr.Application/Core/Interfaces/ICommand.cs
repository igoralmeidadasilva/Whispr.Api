using MediatR;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Core.Abstractions;

public interface ICommand<TValue> : IRequest<Result<TValue>>;