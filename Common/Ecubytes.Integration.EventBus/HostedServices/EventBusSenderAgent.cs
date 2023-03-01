using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ecubytes.Integration.Event;
using Ecubytes.Integration.EventBus.Abstractions;
using Ecubytes.Integration.Queue;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ecubytes.Integration.EventBus.HostedServices
{
    public class EventBusSenderAgent : BackgroundService
    {
        private const string INTEGRATION_EVENT_SUFFIX = "IntegrationEvent";
        private readonly IEventBus eventBus;
        private readonly ILogger logger;
        private readonly IQueueConfigureOptionsProvider queueConfigureOptionsProvider;
        private readonly IQueueSenderService queueSenderService;
        private IIntegrationEventLogRepository integrationEventLogRepository;
        private Dictionary<string, Task> queueTasks = new Dictionary<string, Task>();

        public EventBusSenderAgent()
        {
        }

        public EventBusSenderAgent(IEventBus eventBus,
            IQueueConfigureOptionsProvider queueConfigureOptionsProvider,
            ILogger<EventBusSenderAgent> logger,
            IIntegrationEventLogRepository integrationEventLogRepository,
            IQueueSenderService queueSenderService)
        {
            this.eventBus = eventBus;
            this.logger = logger;
            this.queueConfigureOptionsProvider = queueConfigureOptionsProvider;
            this.queueSenderService = queueSenderService;
            this.integrationEventLogRepository = integrationEventLogRepository;
        }
    
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            if (this.integrationEventLogRepository != null)
            {
                var baseOptions = queueConfigureOptionsProvider.GetBaseOptions();
                var queuesOptions = queueConfigureOptionsProvider.GetAll();
                int queueCount = queuesOptions.Count();

                if (queuesOptions.Any(p => p.EnableSend))
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var logs = (await integrationEventLogRepository.RetrieveEventLogsPendingToPublishAsync()).ToList();

                        foreach (var log in logs)
                        {
                            await integrationEventLogRepository.MarkEventAsInProgressAsync(log.EventId);
                            try
                            {
                                await eventBus.PublishAsync(log.IntegrationEvent);
                                await integrationEventLogRepository.MarkEventAsPublishedAsync(log.EventId);
                            }
                            catch (Exception ex)
                            {
                                await integrationEventLogRepository.MarkEventAsFailedAsync(log.EventId);
                                this.logger.LogError(ex, $"Error when retrying send event to Bus EventId :{log.EventId} for {log.EventTypeName}");                                
                            }                            
                        }

                        // var listEventType = logs.Select(p=>p.EventTypeName).Distinct();

                        // foreach(string eventTypeName in listEventType)
                        // {
                        //     while(logs.Any(p=> p.EventTypeName == eventTypeName))
                        //     {
                        //         eventBus.Publish(logs[0].IntegrationEvent);
                        //     }
                        // }                        
                        await Task.Delay(TimeSpan.FromSeconds(baseOptions.SendWaitTimeSeconds));
                    }
                }
                
            }
        }

        // private async Task QueueTaskAsync(QueueConfigureOptions options)
        // {

        //     ReceiveMessageResponse response = await queueSenderService.SendBatchMessagesAsync(options);

        //     if (response.Status == ReceiveMessageStatus.Ok)
        //     {
        //         foreach (var message in response.Messages)
        //         {
        //             string eventName = $"{message.Name}{INTEGRATION_EVENT_SUFFIX}";

        //             // Complete the message so that it is not received again.
        //             await eventBus.ProcessEventAsync(eventName, message.Body);
        //         }
        //     }
        //     else
        //     {
        //         logger.LogError($"Queue Service returned an invalid state. QueueName: {options.QueueName}");
        //     }
        // }
    }
}
