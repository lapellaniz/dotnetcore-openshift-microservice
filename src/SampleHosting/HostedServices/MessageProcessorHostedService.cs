using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleHosting.Configuration;

namespace SampleHosting.HostedServices
{
    public class MessageProcessorHostedService : BackgroundService
    {
        private readonly ILogger _logger;
        private IQueueClient _queueClient;
        private readonly MessageProcessorOptions _options;
        private readonly IServiceProvider _services;

        public MessageProcessorHostedService(ILogger<MessageProcessorHostedService> logger, MessageProcessorOptions options, IServiceProvider services)
        {
            _logger = logger;
            _options = options;
            _services = services;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _queueClient = new QueueClient(_options.ConnectionString, _options.QueueName, ReceiveMode.PeekLock);

            // Register QueueClient's MessageHandler and receive messages in a loop
            RegisterOnMessageHandlerAndReceiveMessages();

            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            if (_queueClient != null)
            {
                await _queueClient.CloseAsync();
            }
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            using (var scope = _services.CreateScope())
            {
                // Process the message
                var service = scope.ServiceProvider.GetRequiredService<IMessageProcessingService>();
                await service.DoWork(message);
            }

            // Complete the message so that it is not received again.
            // This can be done only if the queueClient is opened in ReceiveMode.PeekLock mode (which is default).
            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            _logger.LogError(exceptionReceivedEventArgs.Exception, "Message handler encountered an exception.");
            return Task.CompletedTask;
        }

        private void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the MessageHandler Options in terms of exception handling, number of concurrent messages to deliver etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of Concurrent calls to the callback `ProcessMessagesAsync`, set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False value below indicates the Complete will be handled by the User Callback as seen in `ProcessMessagesAsync`.
                AutoComplete = false
            };

            // Register the function that will process messages
            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }
    }
}
