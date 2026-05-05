using MediatR;
using Whispr.Application.Core.Abstractions;
using Whispr.SharedKernel.Results;

namespace Whispr.Application.Core.Interfaces;

internal interface ICommandHandler<in TRequest, TValue> : IRequestHandler<TRequest, Result<TValue>>
    where TRequest : ICommand<TValue>;