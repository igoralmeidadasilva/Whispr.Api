namespace Whispr.Presentation.Web;

public static class Routes
{
    public static class External
    {
        public const string MyGitHubUrl = "https://github.com/igoralmeidadasilva";
    }

    public static class Api
    {
        private const string Root = "/api/v1";

        public static class Auth
        {
            private const string Base = Root + "/auth";
            public const string Login = Base + "/login";
            public const string LoginWithGoogle = Base + "/login/google";
            public const string Refresh = Base + "/refresh";
            public const string Logout = Base + "/logout";
        }

        public static class Users
        {
            private const string Base = Root + "/users";
            public const string GetAll = Base;
            public const string GetById = Base + "/{userId:Guid}";
            public const string Create = Base;
            public const string Update = Base + "/{userId:Guid}";
            public const string Delete = Base + "/{userId:Guid}";
            public const string CreatePasswordRecoveryCode = Base + "/password";
            public const string ChangePassword = Base + "/password";
        }
    }

    public static class Web
    {
        public const string Home = "/";
        public const string Contact = "/contact";
        public const string About = "/about";
        public const string Login = "/login";
        public const string Register = "/register";
        public const string ForgotPassword = "/forgot-password";
    }
}