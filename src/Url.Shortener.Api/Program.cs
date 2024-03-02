using Url.Shortener.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Configure(builder.Environment);
builder.ConfigureSerilogAsOnlyLoggingProvider(builder.Configuration);
builder.Services.RegisterServices(builder.Configuration, builder.Environment);

var app = builder.Build();

app.ConfigureMiddleware();

app.Run();

public partial class Program
{ }