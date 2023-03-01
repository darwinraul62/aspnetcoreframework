using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Ecubytes.AspNetCore.Http;
using Ecubytes.AspNetCore.WebUtilities.Api;
using Ecubytes.Data;
using Ecubytes.Identity.Models;

namespace Ecubytes.Identity.Services
{
    internal class LoginService
    {
        private HttpClient httpClient;
        public LoginService(HttpClient httpClient, string baseAddres, string clientId, string clientSecret)
        {
            this.httpClient = httpClient;
            this.BaseAddress = baseAddres;
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
        }

        public string BaseAddress { get; private set; }
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }

        public async Task<ModelResult<LoginResponseDTO>> LoginAsync(string logonName, string password, Guid tenantId)
        {
            ModelResult<LoginResponseDTO> result = new ModelResult<LoginResponseDTO>();

            LoginRequestDTO requestModel = new LoginRequestDTO()
            {
                LogonName = logonName,
                Password = password
            };

            HttpResponseMessage response = null; 
            
            await HttpTransientErrorPolicyExtensions.GetWaitAndRetryPolicy().ExecuteAndCaptureAsync(async () =>
            {                                
                response = await httpClient.PostAsJsonAsync(
                                APIUri.Login.CreateLogin(this.BaseAddress), requestModel);
               
                return response;
            });

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result = await response.Content.ReadAsModelResultAsync<LoginResponseDTO>();
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {    
                result.Messages.AddRange((await response.Content.ReadAsModelResultAsync()).Messages);
            }
            else
                response.EnsureSuccessStatusCode();

            return result;
        }

        public Task<long> LoginCountAsync(LoginCountRequest request)        
        {
            return httpClient.GetAsync<long>(APIUri.Login.LoginCount(BaseAddress, request.DateFrom, request.DateTo, request.UserId));
        }
        public Task<int> UserOnlineCountAsync()        
        {
            return httpClient.GetAsync<int>(APIUri.Login.UserOnlineCount(BaseAddress));
        }
        public async Task SetLastAccessAsync(Guid userId)
        {
            var response = await httpClient.PostAsync(APIUri.Login.SetLastAccess(BaseAddress,userId), null);
            response.EnsureSuccessStatusCode();
        }
    }
}
