using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Hosting;

namespace SampleHosting.Extensions
{
    public static class LoggingExtensions
    {
        public static IHostBuilder UseLogging(this IHostBuilder builder)
        {
            //return builder.UseSerilog((context, loggerBuilder) =>
            //{
            //    loggerBuilder.ReadFrom.Configuration(context.Configuration);
            //});
            builder.ConfigureServices((Action<HostBuilderContext, IServiceCollection>)((context, collection) =>
            {
                var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(context.Configuration)
                    .CreateLogger();
                collection.AddSingleton(services => (ILoggerFactory)new SerilogLoggerFactory(logger, false));
            }));
            return builder;
        }
    }
}