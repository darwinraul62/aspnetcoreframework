using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ecubytes.Cryptography;
using Ecubytes.Data;
using Ecubytes.Identity.User.Api.Models;
using Ecubytes.Identity.User.Data.Models;
using Ecubytes.Identity.User.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ecubytes.Identity.User.Api.Controllers
{
    [ApiController]
    [Route("api/otp")]
    public class OtpController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private IUserRepositoryContainer repositoryContainer;
        public OtpController(IUserRepositoryContainer repositoryContainer)
        {
            this.repositoryContainer = repositoryContainer;
        }

        [HttpPost]
        [Route("generate")]
        [ProducesResponseType(typeof(OtpResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RequestOtp(
            [FromHeader, Required] Guid tenantId,
            [FromBody, Required] OtpRequestDTO requestDTO)
        {
            string plainTextPassword = GenerateRandomOTP(6, new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" });

            Otp model = new Otp();
            if(requestDTO.UserId.HasValue)
            {
                var user = await repositoryContainer.User.GetFirstOrDefaultAsync(p => p.UserId == requestDTO.UserId);

                if (user == null || user.TenantId != tenantId || !user.IsValid)
                    return BadRequestModelResult("Invalid User");

                model.UserId = requestDTO.UserId.Value;
            }
            else
            {
                if(string.IsNullOrWhiteSpace(requestDTO.Challenge))
                    return AddMessageRequiredField("Challenge").BadRequestModelResult();

                model.Challenge = requestDTO.Challenge;
            }
            
            model.TenantId = tenantId;
            model.CreationDate = DateTime.UtcNow;
            model.ExpirationDate = DateTime.UtcNow.AddMinutes(20);
            model.Id = Guid.NewGuid();
            model.Password = EncodePassword(model.Id, plainTextPassword);
            model.StateId = "unused";
            model.TargetId = requestDTO.TargetId;            

            this.repositoryContainer.Otp.Add(model);

            await this.repositoryContainer.SaveChangesAsync();

            return Ok(new OtpResponseDTO()
            {
                Password = plainTextPassword
            });
        }

        [HttpPost]
        [Route("activate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Activate(
            [FromHeader, Required] Guid tenantId,
            [FromBody, Required] OtpActivateRequestDTO requestDTO)
        {
            DateTime dateSearchFrom = DateTime.UtcNow.AddDays(-1);

            Otp otp = null;
            if (requestDTO.UserId.HasValue)
                otp = await repositoryContainer.Otp.GetPendingByUserId(tenantId, requestDTO.TargetId,
                    requestDTO.UserId.Value, dateSearchFrom);
            else
                otp = await repositoryContainer.Otp.GetPendingByChallenge(tenantId, requestDTO.TargetId,
                    requestDTO.Challenge, dateSearchFrom);

            if (otp == null)
                return this.AddErrorMessage("The password is invalid", 100).UnprocessableEntityModelResult();

            if (DateTime.UtcNow > otp.ExpirationDate)
                return this.AddErrorMessage("The password has expired", 101).UnprocessableEntityModelResult();

            string hashPassword = EncodePassword(otp.Id, requestDTO.Password);

            if (otp.Password != hashPassword)
                return this.AddErrorMessage("The password is invalid", 102).UnprocessableEntityModelResult();

            otp.StateId = "used";
            otp.ActivationDate = DateTime.UtcNow;

            this.repositoryContainer.Otp.Update(otp);

            await this.repositoryContainer.SaveChangesAsync();

            return NoContent();
        }

        #region methods

        private string EncodePassword(Guid id, string password)
        {
            return CryptoHelper.GetHashString(String.Concat(id.ToString(), password));
        }

        private string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
        {
            string sOTP = String.Empty;

            string sTempChars = String.Empty;

            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }

            return sOTP;

        }

        #endregion
    }
}
