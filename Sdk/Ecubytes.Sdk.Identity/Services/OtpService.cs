using System;
using System.Net.Http;
using System.Threading.Tasks;
using Ecubytes.AspNetCore.Http;
using Ecubytes.Data;
using Ecubytes.Identity.Models;

namespace Ecubytes.Identity.Services
{
    internal class OtpService
    {
        private readonly HttpClient httpClient;
        public OtpService(HttpClient httpClient, string baseAddres, string clientId, string clientSecret)
        {
            this.httpClient = httpClient;
            this.BaseAddress = baseAddres;
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
        }
        public string BaseAddress { get; private set; }
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }

        public async Task<ModelResult<OtpResponseDTO>> GenerateAsync(Ecubytes.Identity.Models.OtpRequestDTO request)        
        {
            ModelResult<OtpResponseDTO> result = new ModelResult<OtpResponseDTO>();

            HttpResponseMessage response = null; 
            
            await HttpTransientErrorPolicyExtensions.GetWaitAndRetryPolicy().ExecuteAndCaptureAsync(async () =>
            {                                
                response = await httpClient.PostAsJsonAsync(
                                APIUri.Otp.Generate(this.BaseAddress), request);
               
                return response;
            });

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result.Data = await response.Content.ReadAsAsync<OtpResponseDTO>();
            }
            else
                response.EnsureSuccessStatusCode();

            return result;
        } 

        public async Task<ModelResult> ActivateAsync(Ecubytes.Identity.Models.OtpActivateRequestDTO request)        
        {
            ModelResult result = new ModelResult();

            HttpResponseMessage response = null; 
            
            await HttpTransientErrorPolicyExtensions.GetWaitAndRetryPolicy().ExecuteAndCaptureAsync(async () =>
            {                                
                response = await httpClient.PostAsJsonAsync(
                                APIUri.Otp.Activate(this.BaseAddress), request);
               
                return response;
            });

            if(response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)            
                result = await response.Content.ReadAsModelResultAsync<ModelResult>();            
            else
                response.EnsureSuccessStatusCode();

            return result;
        }

    }
}
