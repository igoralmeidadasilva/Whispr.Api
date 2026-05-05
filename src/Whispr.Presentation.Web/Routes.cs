namespace Whispr.Presentation.Web;

public static class Routes
{
    public static class External
    {
        public const string MyGitHubUrl = "https://github.com/igoralmeidadasilva";
    }

    public static class Api
    {
        public const string Base = "/api/v1";
        public const string Users = Base + "/users";
    }

    public static class Web
    {
        public const string Home = "/";
        public const string Contact = "/contact";
        public const string About = "/about";
        public const string Login = "/login";
        public const string Register = "/register";
    }
}