// using System;
// using Ecubytes.AspNetCore.Authorization.Server.Api;
// using Ecubytes.AspNetCore.Authorization.Server.Api.IntegrationEvents.EventHandling;
// using Ecubytes.Integration.EventBus.Abstractions;
// using Microsoft.AspNetCore.Builder;

// namespace Microsoft.Extensions.DependencyInjection
// {
//     public static class IntegrationEventSubscriptionsExtensions
//     {
//         public static void AddIntegrationEventHandlers(this IServiceCollection serviceCollection)
//         {
//             serviceCollection.AddTransient<IdentityApplicationUpdatedIntegrationEventHandler>();
//         }
        
//         public static void AddIntegrationEventSubscriptions(this IApplicationBuilder app)
//         {
//             var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            
//             eventBus.Subscribe<IdentityApplicationUpdatedIntegrationEvent,IdentityApplicationUpdatedIntegrationEventHandler>();
//         }
//     }
// }