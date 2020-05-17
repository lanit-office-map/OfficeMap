using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                  webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                  {
                    var settings = config.Build();
                    if (!string.IsNullOrWhiteSpace(settings["AppConfig"]))
                    {
                      config.AddAzureAppConfiguration(settings["AppConfig"]);
                    }
                  });
                  webBuilder.UseStartup<Startup>();
                });
    }
}