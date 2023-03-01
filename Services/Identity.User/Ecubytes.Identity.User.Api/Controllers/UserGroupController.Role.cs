using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ecubytes.Data;
using Ecubytes.Identity.User.Api.Models;
using Ecubytes.Identity.User.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecubytes.Identity.User.Api.Controllers
{
    public partial class UserGroupController
    {
        [HttpGet]
        [Route("{userGroupId}/roles")]
        [ProducesResponseType(typeof(IEnumerable<RoleDetailViewModelDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRoles([FromRoute] Guid userGroupId,
            [FromHeader, Required] Guid tenantId)
        {
            var details = await this.repositoryContainer.UserGroupRole.GetDetailAsync(tenantId, userGroupId);
            
            return Ok(details.ToDTO());
        }

        [HttpPost]
        [Route("{userGroupId}/roles")]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> InsertRole([FromRoute] Guid userGroupId,
            [FromHeader, Required] Guid tenantId,
            [FromBody] UserGroupRoleInsertRequestDTO requetModel)
        {
            try
            {
                if (!await (ValidateIfUserGroupExists(userGroupId, tenantId)))
                    return NotFoundModelResult();

                if (!(await repositoryContainer.Role.ExistsAsync(p => p.RoleId == requetModel.RoleId)))
                    return AddMessageInvalidFieldValue("Role", "RoleId").
                        BadRequestModelResult();

                UserGroupRole model = new UserGroupRole();
                model.UserGroupId = userGroupId;
                model.RoleId = requetModel.RoleId.Value;

                if (!(await this.repositoryContainer.UserGroupRole.ExistsAsync(
                    p => p.UserGroupId == userGroupId && p.RoleId == requetModel.RoleId)))
                {
                    this.repositoryContainer.UserGroupRole.Add(model);
                    await this.repositoryContainer.SaveChangesAsync();
                }
                return OkModelResult("Role added to user group");
            }
            catch (Exception ex)
            {
                return this.InternalServerErrorModelResult(ex);
            }
        }

        [HttpDelete]
        [Route("{userGroupId}/roles/{roleId}")]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteRole([FromRoute] Guid userGroupId,
            [FromRoute] Guid roleId,
            [FromHeader, Required] Guid tenantId)
        {
            try
            {
                if (!await (ValidateIfUserGroupExists(userGroupId, tenantId)))
                    return NotFoundModelResult();

                if (!(await repositoryContainer.Role.ExistsAsync(p => p.RoleId == roleId)))
                    return AddMessageInvalidFieldValue("Role", "RoleId").
                        BadRequestModelResult();

                UserGroupRole model = await this.repositoryContainer.UserGroupRole.GetFirstOrDefaultAsync(p =>
                    p.UserGroupId == userGroupId && p.RoleId == roleId);

                if (model != null)
                {
                    this.repositoryContainer.UserGroupRole.Remove(model);
                    await this.repositoryContainer.SaveChangesAsync();
                }

                return OkModelResult("Role was removed from user group");
            }
            catch (Exception ex)
            {
                return this.InternalServerErrorModelResult(ex);
            }
        }
    }
}