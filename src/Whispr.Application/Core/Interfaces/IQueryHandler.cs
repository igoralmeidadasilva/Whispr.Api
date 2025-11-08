using MediatR;
using Whispr.Application.Core.Abstractions;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Core.Interfaces;

internal interface IQueryHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse> 
    where TRequest : IQuery<TResponse>
    where TResponse : IBaseResult;