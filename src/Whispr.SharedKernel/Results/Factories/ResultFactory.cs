using System.Reflection;

namespace Whispr.SharedKernel.Results.Factories;

public sealed class ResultFactory<TResponse> : IResultFactory<TResponse> where TResponse : IBaseResult
{
    public TResponse Failure(Error error)
    {
        var resultType = typeof(TResponse);

        var failureMethod = resultType.GetMethod(
            "Failure",
            BindingFlags.Public | BindingFlags.Static,
            null,
            [typeof(Error)],
            null
        );

        if (failureMethod is null || failureMethod.ReturnType != resultType)
        {
            throw new InvalidOperationException($"Cannot create a failure result for type {resultType.Name}. A static 'Failure(Error)' method returning '{resultType.Name}' was not found.");
        }

        var result = failureMethod.Invoke(null, [error]);

        return (TResponse)result!;
    }
}