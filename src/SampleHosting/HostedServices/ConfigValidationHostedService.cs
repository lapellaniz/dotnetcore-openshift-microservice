using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleHosting.Configuration;
using SampleHosting.Configuration.Validation;

namespace SampleHosting.HostedServices {
    public class ConfigValidationHostedService : IHostedService {
        private readonly ILogger _logger;
        private readonly ImmutableList<IValidatable> _options;

        public ConfigValidationHostedService (ILogger<ConfigValidationHostedService> logger, IEnumerable<IValidatable> options) {
            _logger = logger;
            _options = options.ToImmutableList();
        }

        public Task StartAsync (CancellationToken cancellationToken) {
            bool optionsValid = true;
            foreach (var option in _options) {
                ICollection<ValidationResult> results;
                if (!DataAnnotationsValidator.TryValidate (option, out results)) {
                    optionsValid = false;
                    results.ToList ().ForEach (result => _logger.LogError ($"Configuration option {option.GetType().FullName} is invalid. {result.ErrorMessage}. Members: {result.MemberNames}."));
                }
            }

            if (!optionsValid) {
                throw new Exception ("Application configuration is not valid.");
            }

            return Task.CompletedTask;
        }

        public Task StopAsync (CancellationToken cancellationToken) {
            return Task.CompletedTask;
        }
    }
}