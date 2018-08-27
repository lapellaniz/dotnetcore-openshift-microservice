using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleHosting.Configuration;

namespace SampleHosting.HostedServices
{
    public static class HostedServicesServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigValidationHostedService(this IServiceCollection services)
        {
            return services.AddHostedService<ConfigValidationHostedService>();
        }

        public static IServiceCollection AddLoggingHostedService(this IServiceCollection services)
        {
            return services.AddHostedService<LoggingHostedService>();
        }

        public static IServiceCollection AddHealthChecks(this IServiceCollection services)
        {
            return services.AddHostedService<HealthCheckHostedService>();
        }

        public static IServiceCollection AddMessageProcessorHostedService(this IServiceCollection services, IConfiguration config)
        {
            return services
                .ConfigureValidatableSetting<MessageProcessorOptions>(config.GetSection("MessageProcessor"))
                .AddScoped<IMessageProcessingService, MessageProcessingService>()
                .AddHostedService<MessageProcessorHostedService>();
        }
    }
}