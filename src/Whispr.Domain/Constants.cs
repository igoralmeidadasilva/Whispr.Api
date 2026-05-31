namespace Whispr.Domain;

public static class Constants
{    
    public static class Constraints
    {
        public static class User
        {
            public const int NameMinLength = 4;
            public const int NameMaxLength = 32;
            public const int EmailMaxLength = 256;
            public const int PasswordMinLength = 4;
            public const int PasswordMaxLength = 32;
            public const string PasswordFormat = "(?=.*[@#$%^&+=])";
            public const int PasswordRecoveryCodeLength = 4;
        }

        public static class RefreshToken
        {
            public const int TokenLength = 64;
        }

        public static class PasswordResetToken
        {
            public const int TokenLength = 64;
            public const int ExpirationMinutes = 15;
            public const int MaxAttempts = 5;
        }
    }
}