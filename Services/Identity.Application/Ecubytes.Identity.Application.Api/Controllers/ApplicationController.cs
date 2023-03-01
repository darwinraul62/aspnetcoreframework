using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Ecubytes.Data;
using Ecubytes.Identity.Application.Api.IntegrationEvents.Events;
using Ecubytes.Identity.Application.Api.Models;
using Ecubytes.Identity.Application.Data.Repositories;
using Ecubytes.Integration.Event;
using Ecubytes.Integration.EventBus.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ecubytes.Identity.Application.Api.Controllers
{
    [ApiController]
    [Route("api/applications")]
    [ApiVersion("1.0")]
    [ApiVersion("0.0", Deprecated = true)]
    public class ApplicationController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private readonly IApplicationRepositoryContainer repositoryContainer;
        // private IQueueSenderService queueSender;
        IIntegrationEventService integrationEventService;

        public ApplicationController(IApplicationRepositoryContainer repositoryContainer,
            IIntegrationEventService integrationEventService)
        {
            this.repositoryContainer = repositoryContainer;
            // this.queueSender = queueSender;
            this.integrationEventService = integrationEventService;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(IEnumerable<ApplicationViewModelDTO>), (int)HttpStatusCode.OK)]
        //[Authorize(Roles = "Application.Read")]
        public async Task<IActionResult> Get()
        {
            Logger.LogInformation("Test Log Information");

            var application = await repositoryContainer.Application.GetAsync();            

            return Ok(application.ToDTO());
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationViewModelDTO), (int)HttpStatusCode.OK)]
        //[Authorize(Roles = "Application.Read")]
        public async Task<IActionResult> Get(Guid id)
        {
            Logger.LogError("Test Log Information");

            this.Logger.LogDebug("Get Application {0}", id);
            var application = await repositoryContainer.Application.GetFirstOrDefaultAsync(p => p.ApplicationId == id);

            if (application != null)
                this.Logger.LogDebug("Application finded {0}", JsonSerialize(application));
            else
            {
                this.Logger.LogDebug("Application Not Found {0}", id);
                return NotFoundModelResult("Message.ApplicationIdNotExists");
            }

            return Ok(application.ToDTO());
        }

        [HttpPost]
        [ProducesResponseType(typeof(ModelResult<ApplicationInsertDTO>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.InternalServerError)]
        //[Authorize(Roles = "Application.Write")]
        public async Task<IActionResult> Insert(ApplicationInsertDTO application)
        {
            try
            {
                application.Name = application.Name.Trim();

                this.Logger.LogDebug("Start Inserting Application {0}", JsonSerialize(application));

                if (await repositoryContainer.Application.ExistsAsync(p => p.Name == application.Name))
                {
                    //this.AddErrorMessage("Message.ApplicationNameAlreadyExists");
                    return InternalServerErrorModelResult("Message.ApplicationNameAlreadyExists");
                }

                Data.Models.App.Application applicationEntity = new Data.Models.App.Application();

                applicationEntity.ApplicationId = Guid.NewGuid();
                applicationEntity.Name = application.Name;
                applicationEntity.HomePageUrl = application.HomePageUrl;
                applicationEntity.PrivacyStatementUrl = application.PrivacyStatementUrl;
                applicationEntity.StateId = (byte)Data.Models.Constants.ApplicationStates.Active;
                applicationEntity.TermsOfServiceUrl = application.TermsOfServiceUrl;

                var applicationUpdateEvent = new IdentityApplicationUpdatedIntegrationEvent()
                {
                    StateId = applicationEntity.StateId,
                    ApplicationId = applicationEntity.ApplicationId,
                    Name = applicationEntity.Name
                };

                await repositoryContainer.ResilientTransactionAsync(async () =>
                {

                    this.repositoryContainer.Application.Add(applicationEntity);

                    await this.repositoryContainer.SaveChangesAsync();

                    await this.integrationEventService.SaveEventAsync(applicationUpdateEvent);

                });

                await this.integrationEventService.PublishThroughEventBusAsync(applicationUpdateEvent);
                

                this.AddSuccessMessage("Message.ApplicationInsertSuccess");

                return OkModelResult(new ApplicationInsertResultDTO()
                {
                    ApplicationId = applicationEntity.ApplicationId
                });
            }
            catch (Exception ex)
            {
                this.AddErrorMessage(ex, "Message.ApplicationInsertError");
            }

            return OkModelResult();
        }

        [HttpPut]
        [ProducesResponseType(typeof(ModelResult<ApplicationInsertDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        //[Authorize(Roles = "Application.Write")]
        public async Task<IActionResult> Edit(ApplicationUpdateDTO application)
        {
            try
            {
                //string jsonApp = System.Text.Json.JsonSerializer.Serialize(application);
                this.Logger.LogDebug("Start Updating Application {0}", JsonSerialize(application));

                var applicationEntity = await repositoryContainer.Application.GetFirstOrDefaultAsync(p =>
                    p.ApplicationId == application.ApplicationId);

                if (applicationEntity == null)
                    return NotFoundModelResult("Message.ApplicationIdNotExists");

                applicationEntity.ApplicationId = application.ApplicationId;
                applicationEntity.Name = application.Name;
                applicationEntity.HomePageUrl = application.HomePageUrl;
                applicationEntity.PrivacyStatementUrl = application.PrivacyStatementUrl;
                applicationEntity.StateId = (byte)application.StateId;
                applicationEntity.TermsOfServiceUrl = application.TermsOfServiceUrl;

                var applicationUpdateEvent = new IdentityApplicationUpdatedIntegrationEvent()
                {
                    StateId = applicationEntity.StateId,
                    ApplicationId = applicationEntity.ApplicationId,
                    Name = applicationEntity.Name
                };

                await repositoryContainer.ResilientTransactionAsync(async () =>
                {
                    repositoryContainer.Application.Update(applicationEntity);
                    await repositoryContainer.SaveChangesAsync();
                    await integrationEventService.SaveEventAsync(applicationUpdateEvent);
                });

                await integrationEventService.PublishThroughEventBusAsync(applicationUpdateEvent);

                return this.OkModelResult("Message.ApplicationUpdateSuccess");
            }
            catch (Exception ex)
            {
                return this.InternalServerErrorModelResult(ex, "ApplicationUpdateError");
            }
        }
    }
}
