using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SampleHosting.HostedServices
{
    public class HealthCheckHostedService : IHostedService
    {
        private readonly IApplicationLifetime _appLifetime;

        public HealthCheckHostedService(IApplicationLifetime appLifetime)
        {
            _appLifetime = appLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(RegisterReadyCheck);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void RegisterReadyCheck()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "healthy.txt");
            File.WriteAllText(path, "OK");
        }
    }
}
