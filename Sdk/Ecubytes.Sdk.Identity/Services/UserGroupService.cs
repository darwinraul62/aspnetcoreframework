using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Ecubytes.AspNetCore.Http;
using Ecubytes.Data;
using Ecubytes.Data.Common;
using Ecubytes.Identity.Models;
using Ecubytes.Identity.Services;

namespace Ecubytes.Identity.Services
{
    internal class UserGroupService
    {
        private readonly HttpClient httpClient;
        public UserGroupService(HttpClient httpClient, string baseAddres, string clientId, string clientSecret)
        {
            this.httpClient = httpClient;
            this.BaseAddress = baseAddres;
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
        }
        public string BaseAddress { get; private set; }
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }

        public async Task<QueryResult<UserGroupViewModelDTO>> GetAsync(QueryRequest queryRequest)
        {
            var response = await this.httpClient.GetAsync<QueryResult<UserGroupViewModelDTO>>(
                APIUri.UserGroup.Get(this.BaseAddress, queryRequest));

            return response;
        }

         public async Task<UserGroupViewModelDTO> GetAsync(Guid userId)
        {
            var response = await this.httpClient.GetAsync<UserGroupViewModelDTO>(
               APIUri.User.Get(this.BaseAddress, userId));

            return response;
        }

        public async Task<ModelResult<UserGroupCreateResponseDTO>> CreateAsync(UserGroupCreateRequestDTO request)
        {
            ModelResult<UserGroupCreateResponseDTO> result = new ModelResult<UserGroupCreateResponseDTO>();

            HttpResponseMessage response = null;

            await HttpTransientErrorPolicyExtensions.GetWaitAndRetryPolicy().ExecuteAndCaptureAsync(async () =>
            {
                response = await httpClient.PostAsJsonAsync(
                               APIUri.UserGroup.Create(this.BaseAddress), request);

                return response;
            });

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                result.Data = await response.Content.ReadAsAsync<UserGroupCreateResponseDTO>();
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                result.Messages.AddRange((await response.Content.ReadAsModelResultAsync()).Messages);
            }
            else
                response.EnsureSuccessStatusCode();

            return result;
        }

        public async Task<ModelResult> UpdateAsync(Guid userGroupId, UserGroupUpdateRequestDTO request)
        {
            ModelResult result = new ModelResult();

            HttpResponseMessage response = null;

            await HttpTransientErrorPolicyExtensions.GetWaitAndRetryPolicy().ExecuteAndCaptureAsync(async () =>
            {
                response = await httpClient.PutAsJsonAsync(
                                APIUri.UserGroup.Update(this.BaseAddress, userGroupId),request);

                return response;
            });

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                result = await response.Content.ReadAsModelResultAsync();
            else            
                response.EnsureSuccessStatusCode();
            
            return result;
        }

        public async Task<ModelResult> DeleteAsync(Guid userGroupId)
        {
            ModelResult result = new ModelResult();

            HttpResponseMessage response = null;

            await HttpTransientErrorPolicyExtensions.GetWaitAndRetryPolicy().ExecuteAndCaptureAsync(async () =>
            {
                response = await httpClient.DeleteAsync(
                                APIUri.UserGroup.Delete(this.BaseAddress, userGroupId));

                return response;
            });

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                result = await response.Content.ReadAsModelResultAsync();
            else            
                response.EnsureSuccessStatusCode();

            return result;            
        }
    }
}
