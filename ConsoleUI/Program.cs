using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

// Goals: DI, Serilog, Settings

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Application Starting");


            //Host Setup
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    
                })
                .UseSerilog()
                .Build();
        }
        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }

        public class GreetingService
        {
            private readonly ILogger<GreetingService> _log;
            private readonly IConfiguration _config;

            public GreetingService(ILogger<GreetingService> log, IConfiguration config)
            {
                _log = log;
                _config = config;
            }

            public void Run()
            {
                for (int i = 0; i < _config.GetValue<int>("LoopTimes"); i++)
                {
                    // use composite formatting instead of string interpolation
                    // because in logging if you have a structure logger
                    // then Serilog will not only log the text, but log the value separately
                    // so you can query logs based on their numbers and you don't have to parse strings
                    _log.LogInformation("Run number {runNumber}", i);
                }
            }
        }

    }
}
