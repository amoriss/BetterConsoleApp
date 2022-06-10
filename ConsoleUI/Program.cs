using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

// Goals: DI, Serilog, Settings

namespace ConsoleUI
{
    partial class Program
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
                    //when we ask for the GreetingService class we will ask for IGreetingService.
                    //when we get ready to test, it allows us to take away dependencies or change/replace
                    //dependencies that won't make changes to production values.
                    //Even if you only have one implementation of your interface, you
                    //will be able to add a second implementation for unit testing
                    services.AddTransient<IGreetingService, GreetingService>();
                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<IGreetingService>(host.Services);
            svc.Run();

        }
        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }

    }
}
