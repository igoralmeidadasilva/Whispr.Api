namespace Whispr.SharedKernel.Results;

public interface IResult
{
    bool IsSuccess { get; }
    IList<Error> Errors { get; }
}