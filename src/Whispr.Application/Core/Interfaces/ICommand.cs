using MediatR;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Core.Abstractions;

public interface ICommand<TResponse> : IRequest<TResponse> where TResponse : IResult;