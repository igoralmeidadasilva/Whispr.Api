namespace Whispr.SharedKernel.Results;

public interface IResult
{
    bool IsSuccess { get; init; }
    Error Error { get; init; }
}