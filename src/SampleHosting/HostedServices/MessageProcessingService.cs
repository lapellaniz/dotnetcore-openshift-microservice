using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;

namespace SampleHosting.HostedServices
{
    public class MessageProcessingService : IMessageProcessingService
    {
        private readonly ILogger _logger;

        public MessageProcessingService(ILogger<MessageProcessingService> logger)
        {
            _logger = logger;
        }

        public Task DoWork(Message message)
        {
            _logger.LogInformation($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");
            return Task.CompletedTask;
        }
    }

    public interface IMessageProcessingService
    {
        Task DoWork(Message message);
    }
}
