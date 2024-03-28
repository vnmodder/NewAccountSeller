using Serilog;
using ILogger = Serilog.ILogger;
using Serilog.Sinks.File;

namespace AccountSeller.Presentation
{
    public class Program
    {
        protected Program() { }

        public static readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        public static readonly string Namespace = typeof(Startup).Namespace;
        public static readonly string AppName = Namespace.Contains('.') ? Namespace[(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1)..] : Namespace;

        public static IConfiguration Configuration { get; set; }

        public static async Task Main(string[] args)
        {
            Configuration = GetConfiguration(args);
            Log.Logger = CreateSerilogLogger(Configuration);
            var host = CreateHostBuilder(args).Build();
            try
            {
                Log.Information($"STARTING APP - DATE START {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss zz}.");
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "CANNOT START APP. PLEASE CHECK INNER DETAIL.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .CaptureStartupErrors(false)
                        .UseStartup<Startup>();
                });

        static IConfiguration GetConfiguration(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Env}.json", true, true)
                .AddCommandLine(args)
                .AddEnvironmentVariables();
            return builder.Build();
        }

        static ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", Program.AppName)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .WriteTo.File(new Serilog.Formatting.Json.JsonFormatter(), configuration["LogFilePath"])
                .ReadFrom.Configuration(configuration);

            return loggerConfiguration.CreateLogger();
        }
    }
}