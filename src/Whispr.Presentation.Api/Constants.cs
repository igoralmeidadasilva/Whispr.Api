namespace Whispr.Presentation.Api;

public static class Constants
{
    public static class Routes
    {
        public static class Shared
        {
            public const string Base = "/api/v{version:apiVersion}";
            public const string Health = "/health";
            public const string Dashboard = "/dashboard";
        }

        public static class User
        {
            public const string Root = Shared.Base + "/users";
            public const string GetAll = "/";
            public const string GetById = "/{userId:Guid}";
            public const string Create = "/";
            public const string Update = "/{userId:Guid}";
            public const string Delete = "/{userId:Guid}";
            public const string PasswordRecoveryCode = "/password/recovery-code";
            public const string PasswordReset = "/password/reset";
        }

        public static class Auth
        {
            public const string Root = Shared.Base + "/auth";
            public const string Login = "/login";
            public const string LoginWithGoogle = "/login/google";
            public const string Refresh = "/refresh";
            public const string Logout = "/logout";
        }
    }

    public static class Hubs
    {
        public const string ChatUrl = "/chat-hub";
    }

    public static class Settings
    {
        public const string RateLimiter = "Fixed";
        public const string CookieRefreshToken = "refresh_token";
    }
}