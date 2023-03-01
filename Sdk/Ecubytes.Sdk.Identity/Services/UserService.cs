using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Ecubytes.Identity.Models;
using System.Linq;
using Ecubytes.AspNetCore.Http;
using Ecubytes.Data;
using Ecubytes.Data.Common;

namespace Ecubytes.Identity.Services
{
    internal class UserService
    {
        private readonly HttpClient httpClient;
        public UserService(HttpClient httpClient, string baseAddres, string clientId, string clientSecret)
        {
            this.httpClient = httpClient;
            this.BaseAddress = baseAddres;
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
        }
        public string BaseAddress { get; private set; }
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }

        public async Task<IEnumerable<UserRoleEffectiveViewModelDTO>> GetEffectiveRolesAsync(Guid userId, Guid applicationId)
        {
            var response = await this.httpClient.GetAsync<IEnumerable<UserRoleEffectiveViewModelDTO>>(
                APIUri.User.GetEffectiveRoles(this.BaseAddress, userId, applicationId));

            return response;
        }

        public async Task<QueryResult<UserViewModelDTO>> GetAsync(QueryRequest queryRequest)
        {
            var response = await this.httpClient.GetAsync<QueryResult<UserViewModelDTO>>(
               APIUri.User.Get(this.BaseAddress, queryRequest));

            return response;
        }

        public async Task<UserViewModelDTO> GetAsync(Guid userId)
        {
            var response = await this.httpClient.GetAsync<UserViewModelDTO>(
               APIUri.User.Get(this.BaseAddress, userId));

            return response;
        }

        public async Task<bool> CheckEmailUsedAsync(string email)
        {
            var response = await this.httpClient.GetAsync<bool>(
               APIUri.User.CheckEmailUsed(this.BaseAddress, email));

            return response;
        }

        public async Task<bool> CheckLogonNameUsedAsync(string logonName)
        {
            var response = await this.httpClient.GetAsync<bool>(
               APIUri.User.CheckLogonNameUsed(this.BaseAddress, logonName));

            return response;
        }

        public async Task<IEnumerable<UserGroupDetailViewModelDTO>> GetUserGroupsAsync(Guid userId)
        {
            var response = await this.httpClient.GetAsync<IEnumerable<UserGroupDetailViewModelDTO>>(
               APIUri.User.GetUserGroups(this.BaseAddress, userId));

            return response;
        }

        public async Task<ModelResult<UserCreateResponseDTO>> CreateAsync(UserCreateRequestDTO request)
        {
            ModelResult<UserCreateResponseDTO> result = new ModelResult<UserCreateResponseDTO>();

            HttpResponseMessage response = null;

            await HttpTransientErrorPolicyExtensions.GetWaitAndRetryPolicy().ExecuteAndCaptureAsync(async () =>
            {
                response = await httpClient.PostAsJsonAsync(
                               APIUri.User.Create(this.BaseAddress), request);

                return response;
            });

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                result.Data = await response.Content.ReadAsAsync<UserCreateResponseDTO>();
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                result.Messages.AddRange((await response.Content.ReadAsModelResultAsync()).Messages);
            }
            else
                response.EnsureSuccessStatusCode();

            return result;
        }

        public async Task<ModelResult> UpdateAsync(Guid userId, UserUpdateRequestDTO request)
        {
            ModelResult result = new ModelResult();

            HttpResponseMessage response = null;

            await HttpTransientErrorPolicyExtensions.GetWaitAndRetryPolicy().ExecuteAndCaptureAsync(async () =>
            {
                response = await httpClient.PutAsJsonAsync(
                                APIUri.User.Update(this.BaseAddress, userId), request);

                return response;
            });

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                result = await response.Content.ReadAsModelResultAsync();
            else
                response.EnsureSuccessStatusCode();

            return result;
        }

        public async Task<ModelResult> ChangePasswordAsync(Guid userId, UserChangePasswordRequestDTO request)
        {
            ModelResult result = new ModelResult();

            HttpResponseMessage response = null;

            await HttpTransientErrorPolicyExtensions.GetWaitAndRetryPolicy().ExecuteAndCaptureAsync(async () =>
            {
                response = await httpClient.PutAsJsonAsync(
                                APIUri.User.ChangePassword(this.BaseAddress, userId), request);

                return response;
            });

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                result = await response.Content.ReadAsModelResultAsync();
            else
                response.EnsureSuccessStatusCode();

            return result;
        }

        public async Task<ModelResult> DeleteAsync(Guid userId)
        {
            ModelResult result = new ModelResult();

            HttpResponseMessage response = null;

            await HttpTransientErrorPolicyExtensions.GetWaitAndRetryPolicy().ExecuteAndCaptureAsync(async () =>
            {
                response = await httpClient.DeleteAsync(
                                APIUri.User.Delete(this.BaseAddress, userId));

                return response;
            });

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                result = await response.Content.ReadAsModelResultAsync();
            else
                response.EnsureSuccessStatusCode();

            return result;
        }

        public async Task<ModelResult> AddUserGroupAsync(Guid userId, UserAddUserGroupRequestDTO requestDTO)
        {
            ModelResult result = new ModelResult();

            HttpResponseMessage response = null;

            await HttpTransientErrorPolicyExtensions.GetWaitAndRetryPolicy().ExecuteAndCaptureAsync(async () =>
            {
                response = await httpClient.PostAsJsonAsync(
                               APIUri.User.AddUserGroup(this.BaseAddress, userId), requestDTO);

                return response;
            });

            if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
            {
                result.Messages.AddRange((await response.Content.ReadAsModelResultAsync()).Messages);
            }
            else
                response.EnsureSuccessStatusCode();

            return result;
        }

        public async Task<ModelResult> RemoveUserGroupAsync(Guid userId, Guid userGroupId)
        {
            ModelResult result = new ModelResult();

            HttpResponseMessage response = null;

            await HttpTransientErrorPolicyExtensions.GetWaitAndRetryPolicy().ExecuteAndCaptureAsync(async () =>
            {
                response = await httpClient.DeleteAsync(
                                APIUri.User.RemoveUserGroup(this.BaseAddress, userId, userGroupId));

                return response;
            });

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict ||
                response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                result = await response.Content.ReadAsModelResultAsync();
            else
                response.EnsureSuccessStatusCode();

            return result;
        }

        public async Task<ModelResult> RemoveAllUserGroupsAsync(Guid userId)
        {
            ModelResult result = new ModelResult();

            HttpResponseMessage response = null;

            await HttpTransientErrorPolicyExtensions.GetWaitAndRetryPolicy().ExecuteAndCaptureAsync(async () =>
            {
                response = await httpClient.DeleteAsync(
                                APIUri.User.RemoveAllUserGroups(this.BaseAddress, userId));

                return response;
            });

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict ||
                response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                result = await response.Content.ReadAsModelResultAsync();
            else
                response.EnsureSuccessStatusCode();

            return result;
        }

        public Task<long> RegisterCountAsync(UserRegisterCountRequest request)        
        {
            return httpClient.GetAsync<long>(APIUri.User.RegisterCount(BaseAddress, request.DateFrom, request.DateTo));
        }

    }
}