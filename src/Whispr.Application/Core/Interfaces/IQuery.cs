namespace Whispr.Application.Core.Interfaces;

public interface IQuery<TValue> : IRequest<Result<TValue>>;