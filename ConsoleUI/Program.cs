using Microsoft.Extensions.Configuration;
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
        }

        static void BuildConfig(IConfiguration builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}
