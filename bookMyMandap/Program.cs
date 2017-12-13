using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace bookMyMandap
{
    public class Program
    {
        //public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        //   .SetBasePath(Directory.GetCurrentDirectory())
        //   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //   .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        //   .Build();

        public static void Main(string[] args)
        {
            //Log.Logger = new LoggerConfiguration()
            //        .ReadFrom.Configuration(Configuration)
            //        .CreateLogger();
            Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
           .Enrich.FromLogContext()
           .WriteTo.RollingFile("Logs\\log-{Date}.txt")
           .CreateLogger();
            try
            {
                Log.Information("Starting web host");
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();
    }
}
