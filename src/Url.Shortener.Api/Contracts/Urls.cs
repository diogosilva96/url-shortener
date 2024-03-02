namespace Url.Shortener.Api.Contracts;

public static class Urls
{
    public static class Swagger
    {
        public const string Index = "swagger/index.html";
    }

    public static class Api
    {
        private const string BasePath = "api";

        public static class Urls
        {
            public const string BasePath = $"{Api.BasePath}/urls";

            public const string Create = BasePath;

            public static string Get(string code) => $"{BasePath}/{code}";

            public static string List(int pageSize, int page) => $"{BasePath}?pageSize={pageSize}&page={page}";

            public static string Redirect(string code) => $"{code}";
        }
    }

    public static class Health
    {
        public const string BasePath = "health";
    }
}