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

namespace Ecubytes.Identity.Application.Api.Controllers
{
    [ApiController]
    [Route("api/applications/{applicationId}/roles")]
    public class RoleController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private readonly IApplicationRepositoryContainer repositoryContainer;
        private IIntegrationEventService integrationEventService;

        public RoleController(IApplicationRepositoryContainer repositoryContainer,
            IIntegrationEventService integrationEventService)
        {
            this.repositoryContainer = repositoryContainer;
            this.integrationEventService = integrationEventService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<RoleViewModelDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromRoute] Guid applicationId,[FromQuery] bool? active = true)
        {            
            if (!await repositoryContainer.Application.ExistsAsync(p => p.ApplicationId == applicationId))                                
                return BadRequestModelResult("Message.InvalidApplication");            

            var query = repositoryContainer.Role.GetQueryable(p => p.ApplicationId == applicationId);

            if(active.HasValue)
                query = query.Where(p=> p.Active == active.Value);

            var model = await repositoryContainer.Role.ToListFromQueryableAsync(query);

            return Ok(model.ToDTO());
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(RoleViewModelDTO), (int)HttpStatusCode.OK)]
        //[Authorize(Roles = "Application.Read")]
        public async Task<IActionResult> Get([FromRoute] Guid applicationId, [FromRoute] Guid id)
        {            
            var model = await repositoryContainer.Role.GetFirstOrDefaultAsync(p => p.RoleId == id);
            
            if (model == null || model.ApplicationId != applicationId)
                return NotFoundModelResult("Message.RoleNotExists");
            
            return Ok(model.ToDTO());
        }

        [HttpPost]        
        [ProducesResponseType(typeof(ModelResult<RoleInsertResponseDTO>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.BadRequest)]
        //[Authorize(Roles = "Application.Write")]
        public async Task<IActionResult> Insert(
            [FromBody] RoleInsertRequestDTO request, 
            [FromRoute] Guid applicationId)
        {
            try
            {                
                if (!await repositoryContainer.Application.ExistsAsync(p => p.ApplicationId == applicationId))                                    
                    return BadRequestModelResult("Message.InvalidApplication");                

                 if(string.IsNullOrWhiteSpace(request.Name))
                    return this.AddMessageRequiredField("Field.Name").BadRequestModelResult();
                
                if(string.IsNullOrWhiteSpace(request.CodeName))
                    return this.AddMessageRequiredField("Field.CodeName").BadRequestModelResult();  

                if (await repositoryContainer.Role.ExistsAsync(p => p.ApplicationId == applicationId 
                    && p.Name == request.Name))
                    return InternalServerErrorModelResult("Message.RoleNameAlreadyExists");                

                if (await repositoryContainer.Role.ExistsAsync(p => p.ApplicationId == applicationId 
                    && p.CodeName == request.CodeName))                
                    return InternalServerErrorModelResult("Message.RoleCodeNameAlreadyExists");                

                Data.Models.Auth.Role model = new Data.Models.Auth.Role();

                model.ApplicationId = applicationId;
                model.Active = true;
                model.CodeName = request.CodeName;
                model.Description = request.Description;
                model.IsRoleGroup = request.IsRoleGroup;
                model.Name = request.Name;
                model.RoleId = Guid.NewGuid();

                // var applicationUpdateEvent = new IdentityApplicationUpdatedIntegrationEvent()
                // {
                //     StateId = applicationEntity.StateId,
                //     ApplicationId = applicationEntity.ApplicationId,
                //     Name = applicationEntity.Name
                // };

                await repositoryContainer.ResilientTransactionAsync(async () =>
                {
                    this.repositoryContainer.Role.Add(model);
                    await this.repositoryContainer.SaveChangesAsync();
                    // await this.integrationEventService.SaveEventAsync(applicationUpdateEvent);
                });

                //await this.integrationEventService.PublishThroughEventBusAsync(applicationUpdateEvent);

                this.AddSuccessMessage("Message.RoleInsertSuccess");

                return OkModelResult(new RoleInsertResponseDTO()
                {
                    ApplicationId = model.ApplicationId,
                    RoleId = model.RoleId
                });
            }
            catch (Exception ex)
            {
                return this.InternalServerErrorModelResult(ex, "Message.RoleInsertError");
            }            
        }

        [HttpPut]        
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.BadRequest)]
        //[Authorize(Roles = "Application.Write")]
        public async Task<IActionResult> Update(
            [FromBody] RoleUpdateRequestDTO request, 
            [FromRoute] Guid applicationId)
        {
            try
            {                
                if (!await repositoryContainer.Application.ExistsAsync(p => p.ApplicationId == applicationId))                                    
                    return BadRequestModelResult("Message.InvalidApplication");                

                if(request.RoleId == Guid.Empty)
                    return this.AddMessageRequiredField("Field.RoleId", "RoleId").BadRequestModelResult();

                if(string.IsNullOrWhiteSpace(request.Name))
                    return this.AddMessageRequiredField("Field.Name").BadRequestModelResult();
                
                if(string.IsNullOrWhiteSpace(request.CodeName))
                    return this.AddMessageRequiredField("Field.CodeName","CodeName").BadRequestModelResult();  

                if (await repositoryContainer.Role.ExistsAsync(p => p.ApplicationId == applicationId
                    && p.RoleId != request.RoleId 
                    && p.Name == request.Name))
                    return InternalServerErrorModelResult("Message.RoleNameAlreadyExists");                

                if (await repositoryContainer.Role.ExistsAsync(p => p.ApplicationId == applicationId 
                    && p.RoleId != request.RoleId
                    && p.CodeName == request.CodeName))                
                    return InternalServerErrorModelResult("Message.RoleCodeNameAlreadyExists");                

                Data.Models.Auth.Role model = await repositoryContainer.Role.GetFirstOrDefaultAsync(p=>p.RoleId == request.RoleId);

                if(model == null || model.ApplicationId != applicationId)
                    return NotFoundModelResult("Message.RoleNotExists");

                model.Active = request.Active;
                model.CodeName = request.CodeName;
                model.Description = request.Description;
                model.IsRoleGroup = request.IsRoleGroup;
                model.Name = request.Name;
                
                // var applicationUpdateEvent = new IdentityApplicationUpdatedIntegrationEvent()
                // {
                //     StateId = applicationEntity.StateId,
                //     ApplicationId = applicationEntity.ApplicationId,
                //     Name = applicationEntity.Name
                // };

                await repositoryContainer.ResilientTransactionAsync(async () =>
                {
                    this.repositoryContainer.Role.Update(model);
                    await this.repositoryContainer.SaveChangesAsync();
                    // await this.integrationEventService.SaveEventAsync(applicationUpdateEvent);
                });

                //await this.integrationEventService.PublishThroughEventBusAsync(applicationUpdateEvent);

                return this.AddSuccessMessage("Message.RoleUpdateSuccess").
                    OkModelResult();
            }
            catch (Exception ex)
            {
                return this.InternalServerErrorModelResult(ex, "Message.RoleUpdateError");
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
                
                Data.Models.Auth.Role model = await repositoryContainer.Role.GetFirstOrDefaultAsync(p=>p.RoleId == id);

                if(model == null || model.ApplicationId != applicationId)
                    return NotFoundModelResult("Message.RoleNotExists");
                
                // var applicationUpdateEvent = new IdentityApplicationUpdatedIntegrationEvent()
                // {
                //     StateId = applicationEntity.StateId,
                //     ApplicationId = applicationEntity.ApplicationId,
                //     Name = applicationEntity.Name
                // };

                await repositoryContainer.ResilientTransactionAsync(async () =>
                {
                    this.repositoryContainer.Role.Remove(model);
                    await this.repositoryContainer.SaveChangesAsync();
                    // await this.integrationEventService.SaveEventAsync(applicationUpdateEvent);
                });

                //await this.integrationEventService.PublishThroughEventBusAsync(applicationUpdateEvent);

                return this.AddSuccessMessage("Message.RoleDeleteSuccess").
                    OkModelResult();
            }
            catch (Exception ex)
            {
                return this.InternalServerErrorModelResult(ex, "Message.RoleDeleteError");
            }            
        }


    }
}
