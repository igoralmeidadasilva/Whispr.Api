namespace Whispr.SharedKernel.Results.Factories;

public interface IResultFactory<TResponse> where TResponse : IBaseResult
{
    TResponse Failure(Error error);
}