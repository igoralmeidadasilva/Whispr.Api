namespace Whispr.Application.Core.Interfaces;

internal interface IQueryHandler<in TRequest, TValue> : IRequestHandler<TRequest, Result<TValue>>
    where TRequest : IQuery<TValue>;