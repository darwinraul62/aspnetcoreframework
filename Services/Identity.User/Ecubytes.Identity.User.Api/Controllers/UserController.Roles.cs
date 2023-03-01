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
    public partial class UserController
    {
        [HttpGet]
        [Route("{userId}/roles")]
        [ProducesResponseType(typeof(IEnumerable<RoleViewModelDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRoles([FromRoute] Guid userId,
            [FromHeader, Required] Guid tenantId)
        {
            var details = await this.repositoryContainer.UserRole.GetDetailAsync(tenantId, userId);             
            return Ok(details.ToDTO());
        }

        [HttpGet]
        [Route("{userId}/roles/effective")]
        [ProducesResponseType(typeof(IEnumerable<UserRoleEffectiveViewModelDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRolesCodeName([FromRoute] Guid userId,
            [FromHeader, Required] Guid tenantId, [FromQuery] Guid? applicationId = null)
        {
            var details = await this.repositoryContainer.UserRole.GetEffectiveRoleCodeNameAsync(tenantId, userId, applicationId);

            List<UserRoleEffectiveViewModelDTO> model = details.Select(p => new UserRoleEffectiveViewModelDTO()
            {
                CodeName = p
            })
            .ToList();

            return Ok(model);
        }

        [HttpPost]
        [Route("{userId}/roles")]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> InsertRole([FromRoute] Guid userId,
            [FromHeader, Required] Guid tenantId,
            [FromBody] UserRoleInsertRequestDTO requetModel)
        {
            try
            {
                if (!await (ValidateIfUserExists(userId, tenantId)))
                    return NotFoundModelResult();

                if (!(await repositoryContainer.Role.ExistsAsync(p => p.RoleId == requetModel.RoleId)))
                    return AddMessageInvalidFieldValue("Role", "RoleId").
                        BadRequestModelResult();

                UserRole model = new UserRole();
                model.UserId = userId;
                model.RoleId = requetModel.RoleId.Value;

                if (!(await this.repositoryContainer.UserRole.ExistsAsync(
                    p => p.UserId == userId && p.RoleId == requetModel.RoleId)))
                {
                    this.repositoryContainer.UserRole.Add(model);
                    await this.repositoryContainer.SaveChangesAsync();
                }
                return OkModelResult("Role added to user");
            }
            catch (Exception ex)
            {
                return this.InternalServerErrorModelResult(ex);
            }
        }

        [HttpDelete]
        [Route("{userId}/roles/{roleId}")]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteRole([FromRoute] Guid userId,
            [FromRoute] Guid roleId,
            [FromHeader, Required] Guid tenantId)
        {
            try
            {
                if (!await (ValidateIfUserExists(userId, tenantId)))
                    return NotFoundModelResult();

                if (!(await repositoryContainer.Role.ExistsAsync(p => p.RoleId == roleId)))
                    return AddMessageInvalidFieldValue("Role", "RoleId").
                        BadRequestModelResult();

                UserRole model = await this.repositoryContainer.UserRole.GetFirstOrDefaultAsync(p =>
                    p.UserId == userId && p.RoleId == roleId);

                if (model != null)
                {
                    this.repositoryContainer.UserRole.Remove(model);
                    await this.repositoryContainer.SaveChangesAsync();
                }

                return OkModelResult("Role was removed from user");
            }
            catch (Exception ex)
            {
                return this.InternalServerErrorModelResult(ex);
            }
        }
    }
}
