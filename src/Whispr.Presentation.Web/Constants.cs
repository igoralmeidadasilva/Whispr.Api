namespace Whispr.Presentation.Web;

public static class Constants
{
    public static class Pagination
    {
        public const int DefaultPageNumber = 1;
        public const int DefaultPageSize = 10;
    }

    public static class LocalStorageKeys
    {
        public const string AuthKey = "auth-token";
    }

    public static class HttpClients
    {
        public static class Headers
        {
            public const string Correlation = "X-Correlation-ID";
        }
        public const string WhisprApi = nameof(WhisprApi);
    }
}