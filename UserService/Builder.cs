using Microsoft.AspNetCore.Hosting;

namespace UserService
{
    public class Builder
    {
        internal static void Run(string[] args, bool logging)
        {
            CreateHostBuilder(args, logging).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, bool logging)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args);

            hostBuilder.ConfigureWebHostDefaults(delegate (IWebHostBuilder webBuilder)
            {
                webBuilder.UseStartup<Startup>();
            });

            if (!logging)
                hostBuilder.ConfigureLogging(delegate (HostBuilderContext context, ILoggingBuilder logging)
                {
                    logging.ClearProviders();
                });

            return hostBuilder;
        }
    }
}
