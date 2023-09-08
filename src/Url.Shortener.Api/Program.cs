using Url.Shortener.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureSerilogAsOnlyLoggingProvider(builder.Configuration);

builder.Services.RegisterServices(builder.Configuration, builder.Environment);

var app = builder.Build();

app.ConfigureMiddleware();

app.Run();
