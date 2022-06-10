using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

// Goals: DI, Serilog, Settings

namespace ConsoleUI
{
    partial class Program
    {
        public class GreetingService : IGreetingService
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
