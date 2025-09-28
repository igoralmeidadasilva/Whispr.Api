namespace Whispr.SharedKernel.Results;

public class Result<TValue> : IResult
{
    public TValue? Value { get; init; }
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess;
    public IList<Error> Errors { get; init; } = [];

    protected Result(TValue? value, bool isSuccess, IList<Error> errors)
    {
        Value = value;
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public Result() { }

    public static Result<TValue> Success() => new(default, true, []);
    public static Result<TValue> Success(TValue value) => new(value, true, []);
    public static Result<TValue> Failure(Error error) => new(default, false, [error]);
    public static Result<TValue> Failure(IList<Error> errors) => new(default, false, errors);
}