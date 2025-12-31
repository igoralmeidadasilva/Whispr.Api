namespace Whispr.Domain;

public static class Constants
{
    public static class Constraints
    {
        public static class User
        {
            public const int UserNameMinLength = 4;
            public const int UserNameMaxLength = 32;
            public const int PasswordMinLength = 4;
            public const int PasswordMaxLength = 32;
            public const string PasswordFormat = "(?=.*[@#$%^&+=])";
        }
    }
}