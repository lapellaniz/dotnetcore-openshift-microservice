using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SampleHosting.Configuration.Validation;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        // /// <summary>
        // /// Add an <see cref="IStartupFilter"/> to the application that invokes <see cref="IValidatable.Validate"/> on all registered objects
        // /// </summary>
        // public static IServiceCollection UseConfigurationValidation(this IServiceCollection services)
        // {
        //     return services.AddTransient<IStartupFilter, SettingValidationStartupFilter>();
        // }

        /// <summary>
        /// Registers a configuration instance which <typeparamref name="TOptions"/> will bind against, and registers as a validatble setting. 
        /// Additionally registers the configuration object directly with the DI container, so can be retrieved without referencing IOptions. 
        /// 
        /// </summary>
        /// <typeparam name="T">The type of options being configured</typeparam>
        /// <param name="services">The <see cref="IServiceCollection "/> to add the services</param>
        /// <param name="configuration">The configuration being bound.</param>
        /// <returns></returns>
        public static IServiceCollection ConfigureValidatableSetting<TOptions>(this IServiceCollection services, IConfiguration configuration )
            where TOptions: class, IValidatable, new()
        {
            services.Configure<TOptions>(configuration);
            services.AddTransient<TOptions>(ctx => ctx.GetRequiredService<IOptionsSnapshot<TOptions>>().Value);
            services.AddTransient<IValidatable>(ctx => ctx.GetRequiredService<IOptionsSnapshot<TOptions>>().Value);
            return services;
        }
    }
}