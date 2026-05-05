namespace Whispr.SharedKernel.Results;

public interface IBaseResult
{
    bool IsSuccess { get; init; }
    Error Error { get; init; }
}

public interface IBaseResult<TSelf> : IBaseResult where TSelf : IBaseResult<TSelf>
{
    static abstract TSelf Failure(Error error);
}