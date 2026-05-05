using MediatR;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Core.Abstractions;

public interface IQuery<TValue> : IRequest<Result<TValue>>;