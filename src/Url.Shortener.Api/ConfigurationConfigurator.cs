namespace Url.Shortener.Api;

public static class WebApplicationBuilderExtensions
{
    public static ConfigurationManager Configure(this ConfigurationManager configurationManager, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            // allow user secrets in development - Note: development is treated as a local environment here as this is just a sample application.
            configurationManager.AddUserSecrets(typeof(Program).Assembly);
        }

        configurationManager.AddEnvironmentVariables("App_");
        
        return configurationManager;
    }
}