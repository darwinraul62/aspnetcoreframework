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
using Microsoft.AspNetCore.Mvc;

namespace Ecubytes.Identity.User.Api.Controllers
{
    public partial class UserController
    {
        [HttpGet]
        [Route("{userId}/groups")]
        [ProducesResponseType(typeof(IEnumerable<UserGroupDetailViewModelDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserGroups([FromRoute] Guid userId,
            [FromHeader, Required] Guid tenantId)
        {
            var details = await this.repositoryContainer.UserGroupDetail.GetDetailAsync(tenantId, userId);

            return Ok(details.ToDTO());
        }

        [HttpPost]
        [Route("{userId}/groups")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> InsertUserGroup([FromRoute] Guid userId,
            [FromHeader, Required] Guid tenantId,
            [FromBody] UserAddUserGroupRequestDTO requetModel)
        {
            if (!await (ValidateIfUserExists(userId, tenantId)))
                return AddMessageInvalidFieldValue("User", "UserId")
                    .UnprocessableEntityModelResult();

            if (!(await repositoryContainer.UserGroup.ExistsAsync(p => p.UserGroupId == requetModel.UserGroupId)))
                return AddMessageInvalidFieldValue("User Group", "UserGroupId")
                    .UnprocessableEntityModelResult();

            UserGroupDetail model = new UserGroupDetail();
            model.UserId = userId;
            model.UserGroupId = requetModel.UserGroupId.Value;

            if (!(await this.repositoryContainer.UserGroupDetail.ExistsAsync(
                p => p.UserId == userId && p.UserGroupId == requetModel.UserGroupId)))
            {
                this.repositoryContainer.UserGroupDetail.Add(model);
                await this.repositoryContainer.SaveChangesAsync();
            }
            return NoContent();
        }

        [HttpDelete]
        [Route("{userId}/groups/{userGroupId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> DeleteUserGroup([FromRoute] Guid userId,
            [FromRoute] Guid userGroupId,
            [FromHeader, Required] Guid tenantId)
        {
            if (!await (ValidateIfUserExists(userId, tenantId)))
                return AddMessageInvalidFieldValue("User", "UserId")
                    .UnprocessableEntityModelResult();

            if (!(await repositoryContainer.UserGroup.ExistsAsync(p => p.UserGroupId == userGroupId)))
                return AddMessageInvalidFieldValue("User Group", "UserGroupId").
                    UnprocessableEntityModelResult();

            UserGroupDetail model = await this.repositoryContainer.UserGroupDetail.GetFirstOrDefaultAsync(p =>
                p.UserId == userId && p.UserGroupId == userGroupId);

            if (model != null)
            {
                this.repositoryContainer.UserGroupDetail.Remove(model);
                await this.repositoryContainer.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpDelete]
        [Route("{userId}/groups")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> DeleteAllUserGroups([FromRoute] Guid userId,
            [FromHeader, Required] Guid tenantId)
        {
            if (!await (ValidateIfUserExists(userId, tenantId)))
                return AddMessageInvalidFieldValue("User", "UserId")
                    .UnprocessableEntityModelResult();
            
            this.repositoryContainer.UserGroupDetail.RemoveRange(await this.repositoryContainer.UserGroupDetail.GetAsync(p=>p.UserId == userId));
            await this.repositoryContainer.SaveChangesAsync();        

            return NoContent();
        }
    }
}
