// using System;
// using System.Threading.Tasks;
// using Ecubytes.Integration.EventBus.Abstractions;
// using Microsoft.Extensions.Logging;
// using OpenIddict.Abstractions;
// using static OpenIddict.Abstractions.OpenIddictConstants;

// namespace Ecubytes.AspNetCore.Authorization.Server.Api.IntegrationEvents.EventHandling
// {
//     public class IdentityApplicationUpdatedIntegrationEventHandler :
//         IIntegrationEventHandler<IdentityApplicationUpdatedIntegrationEvent>
//     {
//         private readonly IOpenIddictApplicationManager manager;
//         private readonly ILogger<IdentityApplicationUpdatedIntegrationEventHandler> logger;
//         public IdentityApplicationUpdatedIntegrationEventHandler(
//             IOpenIddictApplicationManager manager,
//             ILogger<IdentityApplicationUpdatedIntegrationEventHandler> logger
//         )
//         {
//             this.manager = manager;
//             this.logger = logger;
//         }

//         public async Task Handle(IdentityApplicationUpdatedIntegrationEvent @event)
//         {
//             try
//             {
//                 var applicationDescriptor = new OpenIddictApplicationDescriptor
//                 {
//                     ClientId = @event.ApplicationId.ToString(),
//                     ClientSecret = @event.ApplicationId.ToString(),
//                     DisplayName = @event.Name + " Updated",
//                     Type = OpenIddictConstants.ClientTypes.Confidential,
//                     RedirectUris = { new Uri("https://localhost:5010/signin-oidc") },
//                     Permissions =
//                     {
//                         Permissions.Endpoints.Token,
//                         Permissions.GrantTypes.ClientCredentials,
//                         OpenIddictConstants.Permissions.Endpoints.Authorization,
//                         OpenIddictConstants.Permissions.Endpoints.Token,

//                         OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
//                         OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
//                         OpenIddictConstants.Permissions.GrantTypes.RefreshToken,

//                         OpenIddictConstants.Permissions.Scopes.Profile,
//                         "openid",

//                         OpenIddictConstants.Permissions.Prefixes.Scope + "grapeti.category.read",
//                         OpenIddictConstants.Permissions.Prefixes.Scope + "grapeti.category.write",

//                         OpenIddictConstants.Permissions.ResponseTypes.Code
//                     }
//                 };

//                 var applicationModel = await manager.FindByClientIdAsync(applicationDescriptor.ClientId);

//                 if(applicationModel == null)
//                 {
//                     await manager.CreateAsync(applicationDescriptor);             
//                 }
//                 else
//                 {
//                     await manager.PopulateAsync(applicationModel,applicationDescriptor);
//                     await manager.UpdateAsync(applicationModel);
//                 }
//             }
//             catch (Exception ex)
//             {
//                 this.logger.LogError(ex,"Could not update application in Openiddict authorization provider ");
//             }
//         }
//     }
// }
