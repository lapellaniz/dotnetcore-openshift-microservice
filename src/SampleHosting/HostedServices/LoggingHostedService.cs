using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace SampleHosting.HostedServices
{
    public class LoggingHostedService : IHostedService, IDisposable
    {
        private readonly IApplicationLifetime _appLifetime;
        private readonly ILogger _logger;

        public LoggingHostedService(IApplicationLifetime lifetime, ILogger<LoggingHostedService> logger)
        {
            _appLifetime = lifetime;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Registering logging handlers.");
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;            
            _appLifetime.ApplicationStopped.Register(OnStopped);
            return Task.CompletedTask;
        }
        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnStopped()
        {           
            Cleanup();
            TaskScheduler.UnobservedTaskException -= OnUnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
        }
        
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                LogException(ex);
            }
            Cleanup();
        }

        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            if (!e.Observed)
            {
                e.SetObserved();
                LogException(e.Exception);
            }
            Cleanup();
        }

        private void LogException(Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
        }

        private void Cleanup()
        {
            _logger.LogInformation("Flushing logs.");
            Log.CloseAndFlush();
        }

        public void Dispose()
        {
            // No unmanaged resources
        }
    }
}
