using Whispr.Domain.Features.ValueObjects;
using Whispr.SharedKernel.Guard;

namespace Whispr.Domain.Features.Entities.RefreshTokens;

public sealed record TokenHash : ValueObject
{
    public string Value { get; private init; }

    private TokenHash(string value)
    {
        Value = value;
    }

    public static TokenHash Create(string rawHash)
    {
        Ensure.NotEmpty(rawHash, "TokenHash hash cannot be empty.", nameof(rawHash));
        Ensure.MinimumLength(
            rawHash,
            Constants.Constraints.RefreshToken.TokenLength,
            $"TokenHash hash must be at least {Constants.Constraints.RefreshToken.TokenLength} characters.", nameof(rawHash));

        return new(rawHash.ToLowerInvariant());
    }
}