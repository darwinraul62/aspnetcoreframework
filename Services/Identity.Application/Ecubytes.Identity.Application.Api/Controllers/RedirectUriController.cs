using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ecubytes.Data;
using Ecubytes.Identity.Application.Api.Models;
using Ecubytes.Identity.Application.Data.Repositories;
using Ecubytes.Integration.Event;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ecubytes.Identity.Application.Api.Controllers
{
    [Route("api/applications")]
    [ApiController]
    public class RedirectUriController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase    
    {
        private readonly IApplicationRepositoryContainer repositoryContainer;        
        private readonly IIntegrationEventService integrationEventService;

        public RedirectUriController(IApplicationRepositoryContainer repositoryContainer,
            IIntegrationEventService integrationEventService)
        {
            this.repositoryContainer = repositoryContainer;
            this.integrationEventService = integrationEventService;
        }

        [HttpGet]
        [Route("{applicationId}/uris")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(IEnumerable<RedirectUriViewModelDTO>), (int)HttpStatusCode.OK)]
        //[Authorize(Roles = "Application.Read")]
        public async Task<IActionResult> Get()
        {
            var redirectUris = await repositoryContainer.RedirectUri.GetAsync(includeProperties:"Platform");            

            return Ok(redirectUris.ToDTO());
        }

        [HttpGet]
        [Route("{applicationId}/uris/{id}")]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(RedirectUriViewModelDTO), (int)HttpStatusCode.OK)]
        //[Authorize(Roles = "Application.Read")]
        public async Task<IActionResult> Get(Guid applicationId, Guid id)
        {            
            var model = await repositoryContainer.RedirectUri.GetFirstOrDefaultAsync(p => p.RedirectUriId == id,
                includeProperties:"Platform");
            
            if (model == null || model.ApplicationId != applicationId)                          
                return NotFoundModelResult("Message.RedirectUriNotExists");
            
            return Ok(model.ToDTO());
        }

        [HttpPost]
        [Route("{applicationId}/uris")]
        [ProducesResponseType(typeof(ModelResult<RedirectUriInsertResponseDTO>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.BadRequest)]
        //[Authorize(Roles = "Application.Write")]
        public async Task<IActionResult> Insert(
            [FromBody] RedirectUriInsertRequestDTO request, 
            [FromRoute] Guid applicationId)
        {
            try
            {                
                if (await repositoryContainer.RedirectUri.ExistsAsync(p => p.ApplicationId == applicationId 
                    && p.Uri == request.Uri))
                {                    
                    return InternalServerErrorModelResult("Message.RedirectUriAlreadyExists");
                }

                if (!await repositoryContainer.Application.ExistsAsync(p => p.ApplicationId == applicationId))
                {                    
                    return BadRequestModelResult("Message.InvalidApplication");
                }

                Data.Models.Auth.RedirectUri model = new Data.Models.Auth.RedirectUri();

                model.ApplicationId = applicationId;
                model.PlatformId = request.PlatformId;
                model.RedirectUriId = Guid.NewGuid();
                model.Uri = request.Uri;

                // var applicationUpdateEvent = new IdentityApplicationUpdatedIntegrationEvent()
                // {
                //     StateId = applicationEntity.StateId,
                //     ApplicationId = applicationEntity.ApplicationId,
                //     Name = applicationEntity.Name
                // };

                await repositoryContainer.ResilientTransactionAsync(async () =>
                {

                    this.repositoryContainer.RedirectUri.Add(model);

                    await this.repositoryContainer.SaveChangesAsync();

                    // await this.integrationEventService.SaveEventAsync(applicationUpdateEvent);

                });

                //await this.integrationEventService.PublishThroughEventBusAsync(applicationUpdateEvent);

                this.AddSuccessMessage("Message.RedirectUriInsertSuccess");

                return OkModelResult(new RedirectUriInsertResponseDTO()
                {
                    ApplicationId = model.ApplicationId,
                    UriId = model.RedirectUriId
                });
            }
            catch (Exception ex)
            {
                return this.InternalServerErrorModelResult(ex, "Message.RedirectUriInsertError");
            }            
        }

        [HttpDelete]
        [Route("{applicationId}/uris/{id}")]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.InternalServerError)]
        //[Authorize(Roles = "Application.Write")]
        public async Task<IActionResult> Delete(            
            [FromRoute] Guid applicationId,
            [FromRoute] Guid id)
        {
            try
            {                
                var model = await repositoryContainer.RedirectUri.GetFirstOrDefaultAsync(p => p.RedirectUriId == id);

                 if (model == null || model.ApplicationId != applicationId)      
                    return NotFoundModelResult("Message.RedirectUriNotExists");  

                // var applicationUpdateEvent = new IdentityApplicationUpdatedIntegrationEvent()
                // {
                //     StateId = applicationEntity.StateId,
                //     ApplicationId = applicationEntity.ApplicationId,
                //     Name = applicationEntity.Name
                // };

                await repositoryContainer.ResilientTransactionAsync(async () =>
                {

                    this.repositoryContainer.RedirectUri.Remove(model);

                    await this.repositoryContainer.SaveChangesAsync();

                    // await this.integrationEventService.SaveEventAsync(applicationUpdateEvent);

                });

                //await this.integrationEventService.PublishThroughEventBusAsync(applicationUpdateEvent);

                this.AddSuccessMessage("Message.RedirectUriDeleteSuccess");

                return this.OkModelResult();
            }
            catch (Exception ex)
            {
                return this.InternalServerErrorModelResult(ex, "Message.RedirectUriDeleteError");
            }            
        }
    }
}
