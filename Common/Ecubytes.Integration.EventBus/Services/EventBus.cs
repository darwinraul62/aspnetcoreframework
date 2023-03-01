using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Autofac;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Ecubytes.Integration.EventBus.Abstractions;
using Ecubytes.Integration.Queue;
using Ecubytes.Integration.Event;
using System.Collections.Generic;

namespace Ecubytes.Integration.EventBus.Services
{
    public class EventBusService : IEventBus
    {
        private const string INTEGRATION_EVENT_SUFFIX = "IntegrationEvent";
        private readonly string AUTOFAC_SCOPE_NAME = "ecubytes_event_bus";
        private readonly IQueueSenderService queueSenderService;
        private readonly IEventBusSubscriptionsManager subsManager;
        private readonly ILogger logger;
        private readonly ILifetimeScope autofac;

        public EventBusService(IQueueSenderService queueSenderService, IEventBusSubscriptionsManager subsManager,
            ILogger<EventBusService> logger, ILifetimeScope autofac)
        {
            this.queueSenderService = queueSenderService;
            this.subsManager = subsManager;
            this.logger = logger;
            this.autofac = autofac;
        }

        private void GetPublishAttributeProperty(System.Reflection.MemberInfo element,
            out string serviceName, out string queueName)
        {
            serviceName = null;
            queueName = null;

            var attr = (IntegrationEventPublishAttribute[])element.GetCustomAttributes(typeof(IntegrationEventPublishAttribute), false);
            if (attr.Length > 0)
            {
                serviceName = attr[0].Service;
                queueName = attr[0].Queue;
            }
        }

        public async Task PublishAsync(IntegrationEvent @event)
        {
            if (!Attribute.IsDefined(@event.GetType(), typeof(IntegrationEventPublishAttribute)))
            {
                throw new ArgumentException(@"IntegrationEvent argument does not declare 
                    IntegrationEventPublishAttribute required to publish event");
            }

            var eventName = @event.GetType().Name.Replace(INTEGRATION_EVENT_SUFFIX, "");

            string serviceName = null;
            string queueName = null;

            GetPublishAttributeProperty(@event.GetType(), out serviceName, out queueName);

            var response = await queueSenderService.SendMessageAsync(op =>
            {
                op.ServiceName = serviceName;
                op.QueueName = queueName;
                op.Name = eventName;
                op.MessageBody = @event;
            });//.GetAwaiter().GetResult();

            if(response.Status != SendMessageStatus.Ok)
            {
                throw new Exception($"Error Try Send Message '{response.StatusMessage}' for MessageId : {response.Id}");
            }
        }

        // public void Publish(IEnumerable<IntegrationEvent> events)
        // {
        //     Dictionary<string, SendBatchMessagesOptions> dicEvents = new Dictionary<string, SendBatchMessagesOptions>();

        //     foreach (var @event in events)
        //     {
        //         if (!Attribute.IsDefined(@event.GetType(), typeof(IntegrationEventPublishAttribute)))
        //         {
        //             logger.LogError(@"IntegrationEvent argument does not declare 
        //                 IntegrationEventPublishAttribute required to publish event");
        //             continue;
        //         }

        //         var eventName = @event.GetType().Name.Replace(INTEGRATION_EVENT_SUFFIX, "");

        //         string serviceName = null;
        //         string queueName = null;

        //         GetPublishAttributeProperty(@event.GetType(), out serviceName, out queueName);

        //         if (!dicEvents.ContainsKey(eventName))
        //         {
        //             dicEvents[eventName] = new SendBatchMessagesOptions()
        //             {
        //                 ServiceName = serviceName,
        //                 QueueName = queueName,
        //                 Name = eventName
        //             };
        //         }

        //         dicEvents[eventName].MessagesBody.Add(@event.Info.Id.ToString(),@event);                
        //     }

        //     foreach(var request in dicEvents)
        //     {

        //     }
        // }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name.Replace(INTEGRATION_EVENT_SUFFIX, "");

            // var containsKey = subsManager.HasSubscriptionsForEvent<T>();
            // if (!containsKey)
            // {
            //     try
            //     {

            //         // _subscriptionClient.AddRuleAsync(new RuleDescription
            //         // {
            //         //     Filter = new CorrelationFilter { Label = eventName },
            //         //     Name = eventName
            //         // }).GetAwaiter().GetResult();
            //     }
            //     catch (Exception ex)
            //     {
            //         logger.LogError(ex, "ERROR Subscribing to event {EventName}", eventName);
            //     }
            // }

            logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).Name);

            subsManager.AddSubscription<T, TH>();
        }

        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            logger.LogInformation("Subscribing to dynamic event {EventName} with {EventHandler}", eventName, typeof(TH).Name);

            subsManager.AddDynamicSubscription<TH>(eventName);
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name.Replace(INTEGRATION_EVENT_SUFFIX, "");

            // try
            // {
            //     _subscriptionClient
            //      .RemoveRuleAsync(eventName)
            //      .GetAwaiter()
            //      .GetResult();
            // }
            // catch (MessagingEntityNotFoundException)
            // {
            //     _logger.LogWarning("The messaging entity {eventName} Could not be found.", eventName);
            // }

            logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            subsManager.RemoveSubscription<T, TH>();
        }

        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            logger.LogInformation("Unsubscribing from dynamic event {EventName}", eventName);

            subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        // private void RegisterSubscriptionClientMessageHandler()
        // {
        //     _subscriptionClient.RegisterMessageHandler(
        //         async (message, token) =>
        //         {
        //             var eventName = $"{message.Label}{INTEGRATION_EVENT_SUFFIX}";
        //             var messageData = Encoding.UTF8.GetString(message.Body);

        //             // Complete the message so that it is not received again.
        //             if (await ProcessEvent(eventName, messageData))
        //             {
        //                 await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        //             }
        //         },
        //         new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = 10, AutoComplete = false });
        // }

        // private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        // {
        //     var ex = exceptionReceivedEventArgs.Exception;
        //     var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

        //     _logger.LogError(ex, "ERROR handling message: {ExceptionMessage} - Context: {@ExceptionContext}", ex.Message, context);

        //     return Task.CompletedTask;
        // }


        public async Task<bool> ProcessEventAsync(string eventName, string message)
        {
            var processed = false;
            if (subsManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                {
                    var subscriptions = subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        if (subscription.IsDynamic)
                        {
                            var handler = scope.ResolveOptional(subscription.HandlerType) as IDynamicIntegrationEventHandler;
                            if (handler == null) continue;
                            dynamic eventData = JObject.Parse(message);
                            await handler.Handle(eventData);
                        }
                        else
                        {
                            var handler = scope.ResolveOptional(subscription.HandlerType);
                            if (handler == null) continue;
                            var eventType = subsManager.GetEventTypeByName(eventName);
                            var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                        }
                    }
                }
                processed = true;
            }
            return processed;
        }

        public void Dispose()
        {
            subsManager.Clear();
        }
    }

}
