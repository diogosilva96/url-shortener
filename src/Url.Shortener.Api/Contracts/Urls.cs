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
            private const string BasePath = $"{Api.BasePath}/urls";

            public const string Create = BasePath;
            
            public static string Get(string shortUrl) => $"{BasePath}/{shortUrl}";

            public static string Redirect(string shortUrl) => $"{shortUrl}";
        }
    }
}