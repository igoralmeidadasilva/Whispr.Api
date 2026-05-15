using System.Text.RegularExpressions;

namespace Whispr.SharedKernel.Guard;

public static class Ensure
{
    public static void NotEmpty(string value, string message, string argumentName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotNull(object? value, string message, string argumentName)
    {
        if (value == null)
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotNullOrDefault(object? value, string message, string argumentName)
    {
        if (value == null || value == default)
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void MinimumLength(string value, int minLength, string message, string argumentName)
    {
        if (value != null && value.Length < minLength)
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void MaximumLength(string value, int maxLength, string message, string argumentName)
    {
        if (value != null && value.Length > maxLength)
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void MustContainUpperCase(string value, string message, string argumentName)
    {
        if (value != null && !value.Any(char.IsUpper))
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void MustContainLowerCase(string value, string message, string argumentName)
    {
        if (value != null && !value.Any(char.IsLower))
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void MustContainNumber(string value, string message, string argumentName)
    {
        if (value != null && !value.Any(char.IsDigit))
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void Matches(string value, string pattern, string message, string argumentName)
    {
        if (value != null && !Regex.IsMatch(value, pattern))
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void IsTrue(bool condition, string message, string argumentName)
    {
        if (!condition)
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void IsFalse(bool condition, string message, string argumentName)
    {
        if (condition)
        {
            throw new ArgumentException(message, argumentName);
        }
    }
}