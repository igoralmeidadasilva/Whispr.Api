using Whispr.Domain.Core.Services;
using Whispr.Domain.Features.ValueObjects;
using Whispr.SharedKernel.Guard;

namespace Whispr.Domain.Features.Entities.User;

public sealed record Password : ValueObject
{
    public string Hash { get; }

    private Password(string hash)
    {
        Hash = hash;
    }

    public static Password Create(IPasswordHasherService passwordHasher, string plainText)
    {
        Ensure.NotEmpty(plainText, "Plain text cannot be empty.", nameof(plainText));
        Ensure.MinimumLength(
            plainText,
            Constants.Constraints.User.PasswordMinLength,
            $"Plain text must be at least {Constants.Constraints.User.PasswordMinLength} characters long.",
            nameof(plainText));
        Ensure.MaximumLength(
            plainText,
            Constants.Constraints.User.PasswordMaxLength,
            $"Plain text must be at most {Constants.Constraints.User.PasswordMaxLength} characters long.",
            nameof(plainText));
        Ensure.MustContainNumber(
            plainText,
            "Plain text must contain at least one number.",
            nameof(plainText));
        Ensure.MustContainUpperCase(
            plainText,
            "Plain text must contain at least one uppercase letter.",
            nameof(plainText));
        Ensure.MustContainLowerCase(
            plainText,
            "Plain text must contain at least one lowercase letter.",
            nameof(plainText));
        Ensure.Matches(
            plainText,
            Constants.Constraints.User.PasswordFormat,
            "Plain text must contain at least one non-alphanumeric character.",
            nameof(plainText));

        var hash = passwordHasher.Hash(plainText);
        return new Password(hash);
    }

    public override string ToString()
    {
        return "***";
    }
}