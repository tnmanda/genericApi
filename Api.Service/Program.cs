using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Api.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Run();
        }

        public static IWebHost CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            //.UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
            //.CaptureStartupErrors(true)
            //.UseNLog() log4net
            //.UseKestrel()
            /*
            .ConfigureLogging((hostingContext, logging) =>
            {
                // The ILoggingBuilder minimum level determines the
                // the lowest possible level for logging. The log4net
                // level then sets the level that we actually log at.
                logging.AddLog4Net();
                logging.SetMinimumLevel(LogLevel.Debug);
            })
            */
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseApplicationInsights()
            .Build();
    }
}
