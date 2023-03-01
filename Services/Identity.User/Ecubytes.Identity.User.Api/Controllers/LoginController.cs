using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Ecubytes.Data;
using Ecubytes.Identity.User.Api.MessagesCodes;
using Ecubytes.Identity.User.Api.Models;
using Ecubytes.Identity.User.Api.Utils;
using Ecubytes.Identity.User.Data.Models;
using Ecubytes.Identity.User.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecubytes.Identity.User.Api.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class AccountController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private IUserRepositoryContainer repositoryContainer;
        public AccountController(IUserRepositoryContainer repositoryContainer)
        {
            this.repositoryContainer = repositoryContainer;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> SignIn(
            [FromBody] SignInRequestDTO requestModel,
            [FromHeader, Required] Guid tenantId)
        {

            if (!(await repositoryContainer.Tenant.ExistsAsync(p => p.TenantId == tenantId &&
               p.IsValid)))
            {
                return AddMessageInvalidFieldValue("Tenant", "TenantId").
                    BadRequestModelResult();
            }

            Data.Models.User model = await this.repositoryContainer.User.GetFirstOrDefaultAsync(p =>
                p.LogonName == requestModel.LogonName && p.IsValid);

            if (model == null)
                model = await this.repositoryContainer.User.GetFirstOrDefaultAsync(p =>
                    p.Email == requestModel.LogonName && p.IsValid);


            if (model == null || model.TenantId != tenantId || !model.IsValid
                || model.Password != CryptoPassword.EncryptPassword(model.LogonName, requestModel.Password))
            {
                this.AddErrorMessage("The username and / or password are incorrect",
                    (int)LoginCodes.InvalidLogonNameOrPassword);

                return UnauthorizedModelResult();
            }

            if (model.BlockLogin)
            {
                this.AddErrorMessage("User is not allowed to login",
                    (int)LoginCodes.BlockLogin);

                return UnauthorizedModelResult();
            }

            this.repositoryContainer.UserLogin.Add(new UserLogin()
            {
                LogId = Guid.NewGuid(),
                UserId = model.UserId,
                LoginDate = DateTime.UtcNow
            });

            model.LastAccess = DateTime.UtcNow;
            this.repositoryContainer.User.Update(model);

            await this.repositoryContainer.SaveChangesAsync();

            return OkModelResult(new SignInResponseDTO()
            {
                LogonName = model.LogonName,
                Name = model.Name,
                TenantId = model.TenantId,
                UserId = model.UserId
            });
        }

        [HttpGet]
        [Route("count")]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]        
        public async Task<IActionResult> GetLoginCount(
            [FromHeader, Required] Guid tenantId,
            [FromQuery] DateTime dateFrom,
            [FromQuery] DateTime dateTo,
            [FromQuery] Guid? userId
        )
        {
            long count = await this.repositoryContainer.UserLogin.CountAsync(tenantId, dateFrom, dateTo, userId);
            return Ok(count);
        }

        [HttpGet]
        [Route("online/count")]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]        
        public async Task<IActionResult> GetOnlineCount(
            [FromHeader, Required] Guid tenantId
        )
        {
            int count = await this.repositoryContainer.User.OnlineCountAsync(tenantId);
            return Ok(count);
        }

        [HttpPost]        
        [Route("{userId}/lastaccess")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> UpdateLastAccess(
            [FromHeader, Required] Guid tenantId, 
            [FromRoute] Guid userId)
        {
            var userModel = await this.repositoryContainer.User.GetFirstOrDefaultAsync(p => p.TenantId == tenantId && p.UserId == userId);
            userModel.LastAccess = DateTime.UtcNow;

            this.repositoryContainer.User.Update(userModel);

            await this.repositoryContainer.SaveChangesAsync();

            return NoContent();
        }
    }
}
