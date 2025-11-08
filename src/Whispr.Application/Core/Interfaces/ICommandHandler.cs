using MediatR;
using Whispr.Application.Core.Abstractions;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Core.Interfaces;

internal interface ICommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse> 
    where TRequest : ICommand<TResponse>
    where TResponse : IBaseResult;