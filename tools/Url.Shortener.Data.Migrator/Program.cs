using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Url.Shortener.Data;
using Url.Shortener.Data.Migrator;

var hostBuilder = Host.CreateDefaultBuilder(args)
                      .ConfigureLogging((hostBuilder, loggingBuilder) => 
                          loggingBuilder.ClearProviders()
                                        .AddSerilog(new LoggerConfiguration().MinimumLevel.Debug()
                                                                             .ReadFrom.Configuration(hostBuilder.Configuration)
                                                                             .CreateLogger()))
                      .ConfigureAppConfiguration(app => app.AddJsonFile("appsettings.json")
                                                           .AddUserSecrets(typeof(Program).Assembly)) // user secrets can be used for overriding the configuration locally
                      .ConfigureServices((hostContext, services) => 
                      { 
                          services.AddLogging()
                                  .AddDbContext<ApplicationDbContext>(options => 
                                      options.UseNpgsql(hostContext.Configuration.GetConnectionString("UrlShortenerDatabase"), 
                                          x => x.MigrationsAssembly(typeof(Program).Assembly.FullName)))
                                  .AddHostedService<DataMigrator>(); 
                      });

var host = hostBuilder.Build();

await host.RunAsync();