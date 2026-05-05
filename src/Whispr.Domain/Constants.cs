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
        }
    }
}