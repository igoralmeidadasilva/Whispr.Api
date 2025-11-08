using MediatR;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Core.Abstractions;

public interface IQuery<TResponse> : IRequest<TResponse> where TResponse : IBaseResult;