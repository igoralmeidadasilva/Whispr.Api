namespace Whispr.Application.Core.Interfaces;

public interface ICommand<TValue> : IRequest<Result<TValue>>;