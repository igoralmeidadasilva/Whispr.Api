namespace Whispr.SharedKernel.Results;

public class Result<TValue> : IBaseResult
{
    public TValue? Value { get; init; }
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; init; }

    private Result(TValue? value, bool isSuccess, Error error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    public Result()
    {
        IsSuccess = false;
        Error = Error.None();
    }

    public static Result<TValue> Success() => new(default, true, Error.None());
    public static Result<TValue> Success(TValue value) => new(value, true, Error.None());
    public static Result<TValue> Failure(Error error) => new(default, false, error);
}