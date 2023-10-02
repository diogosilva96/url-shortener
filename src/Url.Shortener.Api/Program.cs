using Url.Shortener.Api;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    // allow user secrets in development - Note: development is treated as a local environment here as this is just a sample application.
    builder.Configuration.AddUserSecrets(typeof(Program).Assembly);
}

builder.Host.ConfigureSerilogAsOnlyLoggingProvider(builder.Configuration);

builder.Services.RegisterServices(builder.Configuration, builder.Environment);

var app = builder.Build();

app.ConfigureMiddleware();

app.Run();

public partial class Program { }