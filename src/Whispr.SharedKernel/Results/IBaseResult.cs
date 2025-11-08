namespace Whispr.SharedKernel.Results;

public interface IBaseResult
{
    bool IsSuccess { get; init; }
    Error? Error { get; init; }
}