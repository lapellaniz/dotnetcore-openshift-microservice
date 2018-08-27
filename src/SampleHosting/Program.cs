using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SampleHosting.Extensions;
using SampleHosting.HostedServices;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace SampleHosting
{
    public class Program
    {
        protected Program()
        {

        }

        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureHostConfiguration(config =>
                {
                    Console.WriteLine("1) ConfigureHostConfiguration...");
                    config.SetBasePath(Directory.GetCurrentDirectory())
                        .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                        .AddCommandLine(args);
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    Console.WriteLine("2) ConfigureAppConfiguration...");
                    config
                        .SetBasePath(Directory.GetCurrentDirectory())

                        // used for local development. this is ignored by docker
                        .AddJsonFile("appsettings.json", true)
                        // used for local development. this is ignored by docker. 
                        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true)

                        // k8s volume mount directories. see kubedeploy.yml for configmap/secret definition
                        .AddJsonFile($"/etc/mysamplehosting/config/appsettings.json", true, true)
                        .AddJsonFile($"/etc/mysamplehosting/secrets/appsettings.secrets.json", true, true)
                        .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                        .AddCommandLine(args);
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        config.AddUserSecrets<Program>(optional: true);
                    }
                })
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    Console.WriteLine("3) ConfigureLogging...");
                    //loggingBuilder.AddSerilog(new LoggerConfiguration()
                    //    .ReadFrom.Configuration(context.Configuration)
                    //    .CreateLogger());
                    //loggingBuilder.AddConsole();
                    //loggingBuilder.AddDebug();
                })
                .ConfigureServices((context, services) =>
                {
                    Console.WriteLine("4) ConfigureServices...");
                    services
                        .AddLogging()
                        .AddLoggingHostedService()
                        .AddConfigValidationHostedService()
                        .AddHealthChecks()
                        .AddMessageProcessorHostedService(context.Configuration);
                })
                .UseLogging();

            //using (var host = builder.Build())
            //{
            //    await host.StartAsync();

            //    await host.WaitForShutdownAsync();
            //}

            // shutdown with ctrl+c
            await builder.RunConsoleAsync();
            Console.WriteLine("Shutting down...");
        }
    }
}
