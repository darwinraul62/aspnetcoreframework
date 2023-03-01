using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ecubytes.Data;
using Ecubytes.Identity.Application.Api.Models;
using Ecubytes.Identity.Application.Data.Models.Auth;
using Ecubytes.Identity.Application.Data.Repositories;
using Ecubytes.Integration.Event;
using Microsoft.AspNetCore.Mvc;

namespace Ecubytes.Identity.Application.Api.Controllers
{
    [ApiController]
    [Route("api/applications/{applicationId}/secrets")]
    public class ApplicationSecretController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private readonly IApplicationRepositoryContainer repositoryContainer;
        private readonly IIntegrationEventService integrationEventService;

        public ApplicationSecretController(IApplicationRepositoryContainer repositoryContainer,
            IIntegrationEventService integrationEventService)
        {
            this.repositoryContainer = repositoryContainer;
            this.integrationEventService = integrationEventService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ApplicationSecretViewModelDTO>), (int)HttpStatusCode.OK)]
        //[Authorize(Roles = "Application.Read")]
        public async Task<IActionResult> Get(Guid applicationId)
        {
            var secrets = await repositoryContainer.ApplicationSecret.
                GetAsync(p => p.ApplicationId == applicationId);

            return Ok(secrets.ToDTO());
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationSecretViewModelDTO), (int)HttpStatusCode.OK)]
        //[Authorize(Roles = "Application.Read")]
        public async Task<IActionResult> Get(Guid applicationId, Guid id)
        {
            var secret = await repositoryContainer.ApplicationSecret.
                GetFirstOrDefaultAsync(p => p.ApplicationSecretId == id);

            if (secret == null || secret.ApplicationId != applicationId)
                return BadRequestModelResult("Message.ApplicationSecretNotExists");

            return Ok(secret.ToDTO());
        }

        [HttpPost]        
        [ProducesResponseType(typeof(ModelResult<ApplicationSecretInsertResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.InternalServerError)]
        //[Authorize(Roles = "Application.Read")]
        public async Task<IActionResult> Insert([FromRoute] Guid applicationId,
            [FromBody] ApplicationSecretInsertRequestDTO requestDTO)
        {
            try
            {
                if (!(await this.repositoryContainer.Application.ExistsAsync(p => p.ApplicationId == applicationId)))
                    return BadRequestModelResult("Message.InvalidApplication");

                if ((await this.repositoryContainer.ApplicationSecret.GetActiveSecretCountAsync(applicationId)) >= 2)
                    return InternalServerErrorModelResult("Message.SecretsCountExceeded");

                if(requestDTO.ExpirationDate.Date < DateTime.UtcNow.Date)
                    return BadRequestModelResult("Message.ExpirationDateInvalid");

                ApplicationSecret model = new ApplicationSecret();
                model.ApplicationId = applicationId;
                model.ApplicationSecretId = Guid.NewGuid();
                model.Secret = Cryptography.CryptoHelper.GetHashString(Guid.NewGuid().ToString());
                model.CreationDate = DateTime.UtcNow;
                model.Description = requestDTO.Description;
                model.ExpirationDate = requestDTO.ExpirationDate;

                this.repositoryContainer.ApplicationSecret.Add(model);
                await this.repositoryContainer.SaveChangesAsync();

                this.AddSuccessMessage("Message.ApplicationSecretInsertSuccess");

                return OkModelResult(new ApplicationSecretInsertResponseDTO()
                {
                    ApplicationId = model.ApplicationId,
                    SecretId = model.ApplicationSecretId,
                    Secret = model.Secret
                });
            }
            catch (Exception ex)
            {
                return InternalServerErrorModelResult(ex, "Message.ApplicationSecretInsertError");
            }
        }

        [HttpDelete]    
        [Route("{id}")]    
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        //[Authorize(Roles = "Application.Write")]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid applicationId,
            [FromRoute] Guid id)
        {
            try
            {                
                
                ApplicationSecret model = await repositoryContainer.ApplicationSecret.
                    GetFirstOrDefaultAsync(p=>p.ApplicationSecretId == id);

                if(model == null || model.ApplicationId != applicationId)
                    return NotFoundModelResult("Message.ApplicationSecretNotExists");
                
                // var applicationUpdateEvent = new IdentityApplicationUpdatedIntegrationEvent()
                // {
                //     StateId = applicationEntity.StateId,
                //     ApplicationId = applicationEntity.ApplicationId,
                //     Name = applicationEntity.Name
                // };

                await repositoryContainer.ResilientTransactionAsync(async () =>
                {
                    this.repositoryContainer.ApplicationSecret.Remove(model);
                    await this.repositoryContainer.SaveChangesAsync();
                    // await this.integrationEventService.SaveEventAsync(applicationUpdateEvent);
                });

                //await this.integrationEventService.PublishThroughEventBusAsync(applicationUpdateEvent);

                return this.AddSuccessMessage("Message.ApplicationSecretDeleteSuccess").
                    OkModelResult();
            }
            catch (Exception ex)
            {
                return this.InternalServerErrorModelResult(ex, "Message.ApplicationSecretDeleteError");
            }
        }
    }
}
