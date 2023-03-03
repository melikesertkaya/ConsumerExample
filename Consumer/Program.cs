using Microsoft.Extensions.Configuration;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Serilog.Exceptions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Consumer.Extensions;
using MessageContract.Models;

namespace Consumer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var confUrl = Environment.GetEnvironmentVariable("CONF_URL");
            var organizationCode = Environment.GetEnvironmentVariable("COUNTRY_CODE");
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true).Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["LogUrl"]))
                {
                    CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                    ModifyConnectionSettings = c => c.ServerCertificateValidationCallback((o, certificate, arg3, arg4) => { return true; }),
                    AutoRegisterTemplate = true,
                    IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                })
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            await Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddConfiguration(new ConfigurationOption { ConfigApiUrl = confUrl, OrganizationCode = organizationCode, Environment = environment });
                    services.AddHttpClient();
                    services.ConfigureBus();
                }).UseSerilog().Build().RunAsync();
        }
    }
}
