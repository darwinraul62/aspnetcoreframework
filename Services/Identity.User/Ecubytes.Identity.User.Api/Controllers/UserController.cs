using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ecubytes.Data;
using Ecubytes.Data.Common;
using Ecubytes.Identity.User.Api.Models;
using Ecubytes.Identity.User.Data.Models;
using Ecubytes.Identity.User.Data.Repositories;
using Identity.User.IntegrationEvents;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Ecubytes.Identity.User.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public partial class UserController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private readonly IUserRepositoryContainer repositoryContainer;
        //private readonly IPublishEndpoint publishEndpoint;
        public UserController(IUserRepositoryContainer repositoryContainer,
            IStringLocalizer<DefaultResourceLocalization> defaultLocalizer
            //IPublishEndpoint publishEndpoint
            )
        {
            this.AddLocalizer(defaultLocalizer);
            this.repositoryContainer = repositoryContainer;
            //this.publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        [ProducesResponseType(typeof(QueryResult<UserViewModelDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromHeader, Required] Guid tenantId, 
            [FromQuery] QueryRequest queryRequest)
        {
            // queryRequest.AddFilter("TenantId", tenantId);
            // queryRequest.AddFilter("IsValid", true);

            var result = await this.repositoryContainer.User.GetUsersInfoAsync(queryRequest, 
                p => p.TenantId == tenantId && p.IsValid);

            var response = QueryResult.Convert<UserViewModelDTO>(result, 
                result.Data.ToDTO());

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(UserViewModelDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, [FromHeader, Required] Guid tenantId)
        {
            var result = await this.repositoryContainer.User.GetUsersInfoAsync(QueryRequest.Builder(), p => 
                p.TenantId == tenantId &&
                p.UserId == id);

            var user = result.Data?.FirstOrDefault();

            if (user == null || !user.IsValid)
                return NotFoundModelResult();

            return Ok(user.ToDTO());
        }

        [HttpGet]
        [Route("emails/used")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChekEmailUsed(
            [FromHeader, Required] Guid tenantId, string email)
        {
            var result = await this.repositoryContainer.User.ExistsAsync(p=> 
                p.TenantId == tenantId &&
                p.Email == email);

            return Ok(result);  
        }

        [HttpGet]
        [Route("logonnames/used")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChekLogonNameUsed(
            [FromHeader, Required] Guid tenantId, string logonName)
        {
            var result = await this.repositoryContainer.User.ExistsAsync(p=> 
                p.TenantId == tenantId &&
                p.Email == logonName);

            return Ok(result);  
        }

        [HttpGet]
        [Route("registers/count")]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]        
        public async Task<IActionResult> GetRegisterCount(
            [FromHeader, Required] Guid tenantId,
            [FromQuery] DateTime dateFrom,
            [FromQuery] DateTime dateTo            
        )
        {
            long count = await this.repositoryContainer.User.RegisterCountAsync(tenantId, dateFrom, dateTo);
            return Ok(count);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserInsertResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Insert([FromBody] UserInsertRequestDTO requestModel,
            [FromHeader, Required] Guid tenantId)
        {
            if (!(await this.repositoryContainer.Tenant.ExistsAsync(p => p.TenantId == tenantId)))
                return AddMessageInvalidFieldValue("Tenant", "TenantId").
                    BadRequestModelResult();

            if (await (this.repositoryContainer.User.ExistsAsync(p =>
                 p.TenantId == tenantId &&
                 p.LogonName == requestModel.LogonName &&
                 p.IsValid)))
            {
                return ConflictModelResult("The LogonName already exists");
            }

            if (await (this.repositoryContainer.User.ExistsAsync(p =>
                 p.TenantId == tenantId &&
                 p.LogonName == requestModel.Email &&
                 p.IsValid)))
            {
                return ConflictModelResult("The Email already exists");
            }

            if (requestModel.UserId.HasValue &&
                await (this.repositoryContainer.User.ExistsAsync(p =>
                 p.TenantId == tenantId &&
                 p.LogonName == requestModel.LogonName &&
                 p.IsValid)))
            {
                return ConflictModelResult("The UserId already exists");
            }

            Data.Models.User model = new Data.Models.User();
            model.BlockLogin = requestModel.BlockLogin;
            model.Email = requestModel.Email;
            model.LastNames = requestModel.LastNames;
            model.LogonName = requestModel.LogonName;
            model.Names = requestModel.Names;
            model.Password = Utils.CryptoPassword.EncryptPassword(requestModel.LogonName, requestModel.Password);
            model.StateId = UserStates.Active;
            model.IsValid = true;
            model.TenantId = tenantId;
            model.UserId = !requestModel.UserId.HasValue ? Guid.NewGuid() : requestModel.UserId.Value;
            model.RegisterDate = requestModel.RegisterDate;

            if (requestModel.Attributes != null && requestModel.Attributes.Any())
            {
                model.Attributes = requestModel.Attributes.Select(p => new UserAttribute()
                {
                    UserId = model.UserId,
                    Name = p.Key,
                    Value = p.Value
                }).ToList();
            }

            repositoryContainer.User.Add(model);
            await repositoryContainer.SaveChangesAsync();

            // await publishEndpoint.Publish(new UserCreatedIntegrationEvent()
            // {
            //     BlockLogin = model.BlockLogin,
            //     Email = model.Email,
            //     LastNames = model.LastNames,
            //     LogonName = model.LogonName,
            //     Names = model.Names,
            //     TenantId = model.TenantId,
            //     UserId = model.UserId,
            //     Attributtes = requestModel.Attributes
            // }, p =>
            //  {                 
            //      if (model.Attributes != null)
            //      {
            //          foreach (var at in model.Attributes)
            //              p.Headers.Set(at.Name, at.Value);
            //      }
            //  });

            return CreatedAtAction(nameof(GetById), new { id = model.UserId }, new UserInsertResponseDTO()
            {
                UserId = model.UserId
            });
        }

        [HttpPut]
        [Route("{userId}/password")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ChangePassword(
            ChangePasswordRequestDTO requestDTO,
            [FromHeader, Required] Guid tenantId,
            [FromRoute, Required] Guid userId)
        {
            Data.Models.User model = await repositoryContainer.User.GetFirstOrDefaultAsync(p => p.UserId == userId);

            if (model == null || model.TenantId != tenantId || !model.IsValid)
                return NotFoundModelResult();
            
            model.Password = Utils.CryptoPassword.EncryptPassword(model.LogonName, requestDTO.Password);

            repositoryContainer.User.Update(model);
            await repositoryContainer.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Update(
            [FromBody] UserUpdateRequestDTO requestModel,
            [FromHeader, Required] Guid tenantId,
            [FromRoute, Required] Guid id)
        {

            Data.Models.User model = await repositoryContainer.User.GetFirstOrDefaultAsync(p => p.UserId == id);

            if (model == null || model.TenantId != tenantId || !model.IsValid)
                return NotFoundModelResult();

            if ((await repositoryContainer.User.ExistsAsync(p =>
                p.TenantId == tenantId &&
                p.UserId != id &&
                p.LogonName == requestModel.LogonName &&
                p.IsValid)))
                return ConflictModelResult("The LogonName already exists");

            if ((await repositoryContainer.User.ExistsAsync(p =>
                p.TenantId == tenantId &&
                p.UserId != id &&
                p.Email == requestModel.Email &&
                p.IsValid)))
                return ConflictModelResult("The Email already exists");

            model.BlockLogin = requestModel.BlockLogin;
            model.Email = requestModel.Email;
            model.LastNames = requestModel.LastNames;
            model.LogonName = requestModel.LogonName;
            model.Names = requestModel.Names;

            repositoryContainer.User.Update(model);
            await repositoryContainer.SaveChangesAsync();

            return NoContent();
        }       

        // [HttpPatch]
        // [Route("{id}")]
        // [ProducesResponseType((int)HttpStatusCode.NoContent)]
        // public async Task<IActionResult> UpdatePatch(
        //     [FromHeader, Required] Guid tenantId,
        //     [FromRoute, Required] Guid userId,
        //     [FromBody] JsonPatchDocument<UserUpdateRequestDTO> patchDocument)
        // {
        //     Data.Models.User model = await repositoryContainer.User.GetFirstOrDefaultAsync(p => p.UserId == userId);

        //     if (model == null || model.TenantId != tenantId || !model.IsValid)
        //         return NotFound();
           
        //     var modelApi = mapper.Map<UserUpdateRequestDTO>(model);

        //     patchDocument.ApplyTo(modelApi);

        //     mapper.Map(modelApi, model);

        //     this.repositoryContainer.ProviderResource.Update(model);

        //     await this.repositoryContainer.SaveChangesAsync();

        //     return NoContent();
        // }


        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid id, [FromHeader, Required] Guid tenantId)
        {
            var model = await this.repositoryContainer.User.GetFirstOrDefaultAsync(p => p.UserId == id);

            if (model == null || model.TenantId != tenantId || !model.IsValid)
                return NotFoundModelResult();

            model.StateId = UserStates.Deleted;
            model.IsValid = false;

            this.repositoryContainer.User.Update(model);
            await this.repositoryContainer.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> ValidateIfUserExists(Guid userId, Guid tenantId)
        {
            return (await (this.repositoryContainer.User.ExistsAsync(p => p.UserId == userId && p.TenantId == tenantId
                       && p.IsValid)));
        }

    }
}
