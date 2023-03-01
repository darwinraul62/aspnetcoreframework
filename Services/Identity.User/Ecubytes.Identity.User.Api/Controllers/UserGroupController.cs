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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;

namespace Ecubytes.Identity.User.Api.Controllers
{
    [ApiController]
    [Route("api/usergroups")]
    public partial class UserGroupController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private readonly IUserRepositoryContainer repositoryContainer;
        public UserGroupController(IUserRepositoryContainer repositoryContainer,
            IStringLocalizer<DefaultResourceLocalization> defaultLocalizer)
        {
            this.AddLocalizer(defaultLocalizer);
            this.repositoryContainer = repositoryContainer;
        }

        [HttpGet]
        [ProducesResponseType(typeof(QueryResult<IEnumerable<UserGroupViewModelDTO>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromHeader, Required] Guid tenantId, 
            [FromQuery] QueryRequest queryRequest)
        {
            queryRequest.AddCondition("TenantId", tenantId);
            queryRequest.AddCondition("IsValid", true);

            var result = await this.repositoryContainer.UserGroup.GetAsync(queryRequest);

            var response = QueryResult.Convert<UserGroupViewModelDTO>(result, result.Data.ToDTO());

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(UserGroupViewModelDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid id, [FromHeader, Required] Guid tenantId)
        {
            var user = await this.repositoryContainer.UserGroup.GetFirstOrDefaultAsync(p => p.UserGroupId == id);

            if (user == null || user.TenantId != tenantId || !user.IsValid)
                return NotFoundModelResult();

            return Ok(user.ToDTO());
        }

        [HttpPost]
        [ProducesResponseType(typeof(ModelResult<UserGroupInsertResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Insert([FromBody, BindRequired] UserGroupInsertRequestDTO requestModel,
            [FromHeader, Required] Guid tenantId)
        {
            try
            {
                if (!(await this.repositoryContainer.Tenant.ExistsAsync(p => p.TenantId == tenantId)))
                    return AddMessageInvalidFieldValue("Tenant", "TenantId").
                        BadRequestModelResult();

                Data.Models.UserGroup model = new Data.Models.UserGroup();
                model.Name = requestModel.Name.Trim();
                model.StateId = UserGroupStates.Active;
                model.IsValid = true;
                model.UserGroupId = Guid.NewGuid();
                model.TenantId = tenantId;

                repositoryContainer.UserGroup.Add(model);
                await repositoryContainer.SaveChangesAsync();

                return this.AddSuccessMessage("User Group created successfully").
                    OkModelResult(new UserGroupInsertResponseDTO()
                    {
                        UserGroupId = model.UserGroupId
                    });
            }
            catch (Exception ex)
            {
                return InternalServerErrorModelResult(ex);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Update(
            [FromBody] UserGroupUpdateRequestDTO requestModel,
            [FromHeader, Required] Guid tenantId)
        {
            try
            {
                Data.Models.UserGroup model = await repositoryContainer.UserGroup.GetFirstOrDefaultAsync(p =>
                    p.UserGroupId == requestModel.UserGroupId);

                if (model == null || model.TenantId != tenantId || !model.IsValid)
                    return NotFoundModelResult();

                model.Name = requestModel.Name;

                repositoryContainer.UserGroup.Update(model);
                await repositoryContainer.SaveChangesAsync();

                return this.AddSuccessMessage("User Group updated successfully").
                    OkModelResult();
            }
            catch (Exception ex)
            {
                return InternalServerErrorModelResult(ex);
            }
        }


        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid id, [FromHeader, Required] Guid tenantId)
        {
            try
            {
                var model = await this.repositoryContainer.UserGroup.GetFirstOrDefaultAsync(p => p.UserGroupId == id);

                if (model == null || model.TenantId != tenantId || !model.IsValid)
                    return NotFoundModelResult();

                model.StateId = UserGroupStates.Deleted;
                model.IsValid = false;

                this.repositoryContainer.UserGroup.Update(model);
                await this.repositoryContainer.SaveChangesAsync();

                this.AddSuccessMessage("User Group was Deleted");
                return this.OkModelResult();

            }
            catch (Exception ex)
            {
                return InternalServerErrorModelResult(ex);
            }
        }

        private async Task<bool> ValidateIfUserGroupExists(Guid userGroupId, Guid tenantId)
        {
            return (await (this.repositoryContainer.UserGroup.ExistsAsync(p => p.UserGroupId == userGroupId && p.TenantId == tenantId
                       && p.IsValid)));
        }
    }
}
