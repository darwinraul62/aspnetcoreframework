using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ecubytes.Integration.Queue;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text;
using Ecubytes.Integration.EventBus.Abstractions;

namespace Ecubytes.Integration.EventBus.HostedServices
{
    public class EventBusReceiverAgent : BackgroundService
    {
        private const string INTEGRATION_EVENT_SUFFIX = "IntegrationEvent";
        private readonly IEventBus eventBus;
        private readonly ILogger logger;
        private readonly IQueueConfigureOptionsProvider queueConfigureOptionsProvider;
        private readonly IQueueReceiverService queueReceiverService;
        private Dictionary<string, Task> queueTasks = new Dictionary<string, Task>();
        public EventBusReceiverAgent(IEventBus eventBus,
            IQueueConfigureOptionsProvider queueConfigureOptionsProvider,
            ILogger<EventBusReceiverAgent> logger,
            IQueueReceiverService queueReceiverService)
        {
            this.eventBus = eventBus;
            this.logger = logger;
            this.queueConfigureOptionsProvider = queueConfigureOptionsProvider;
            this.queueReceiverService = queueReceiverService;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var baseOptions = queueConfigureOptionsProvider.GetBaseOptions();
            var queuesOptions = queueConfigureOptionsProvider.GetAll();
            int queueCount = queuesOptions.Count();
            int queueIteration = 0;

            if (queuesOptions.Any(p => p.EnableReceive))
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    foreach (var queueOptions in queuesOptions)
                    {
                        try
                        {
                            queueIteration++;
                            Task queueTask = queueTasks.GetValueOrDefault(queueOptions.QueueName, QueueTaskAsync(queueOptions));

                            if (queueTask.IsCompleted)
                            {
                                queueTask.Dispose();
                                logger.LogDebug($"Start Queue Task {queueOptions.QueueName}");
                                queueTasks[queueOptions.QueueName] = QueueTaskAsync(queueOptions);
                            }

                            if (queueIteration == queueCount)
                            {
                                queueIteration = 0;
                                await Task.Delay(TimeSpan.FromSeconds(baseOptions.ReceiveWaitTimeSeconds));
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"Failed Task for Queue {queueOptions.QueueName}");
                        }
                    }
                }
            }
        }

        private async Task QueueTaskAsync(IQueueSettings options)
        {
            ReceiveMessageResponse response = await queueReceiverService.ReceiveMessagesAsync(options);

            if (response.Status == ReceiveMessageStatus.Ok)
            {
                foreach (var message in response.Messages)
                {
                    string eventName = $"{message.Name}{INTEGRATION_EVENT_SUFFIX}";

                    // Complete the message so that it is not received again.
                    await eventBus.ProcessEventAsync(eventName, message.Body);
                }
            }
            else
            {
                logger.LogError($"Queue Service returned an invalid state. QueueName: {options.QueueName}");
            }
        }
    }
}
