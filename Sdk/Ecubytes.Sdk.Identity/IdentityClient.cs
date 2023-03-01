using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Ecubytes.AspNetCore.Http;
using Ecubytes.Data;
using Ecubytes.Identity.Models;
using Ecubytes.Identity.Services;
using System.Linq;
using Ecubytes.Data.Common;

namespace Ecubytes.Identity
{
    public class IdentityClient : IDisposable
    {
        private HttpClient httpClient;
        private LoginService loginService;
        private UserService userService;
        private OtpService otpService;
        private UserGroupService userGroupService;
        private Guid TenantId;

        public IdentityClient(string baseAddres, string clientId, string clientSecret, Guid tenantId)
        {
            //Alternative to IHttpClientFactory using SocketsHttpHandler for application Lifetime
            httpClient = new HttpClient(GlobalHandlers.GlobalSocketsHttpHandler, disposeHandler: false);

            this.BaseAddress = baseAddres;
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.TenantId = tenantId;

            this.InitServices();
        }

        public string BaseAddress { get; private set; }
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }

        private void InitServices()
        {
            httpClient.DefaultRequestHeaders.Add("TenantId", TenantId.ToString());

            loginService = new LoginService(this.httpClient, BaseAddress, ClientId, ClientSecret);
            userService = new UserService(this.httpClient, BaseAddress, ClientId, ClientSecret);
            userGroupService = new UserGroupService(this.httpClient, BaseAddress, ClientId, ClientSecret);
            otpService = new OtpService(this.httpClient, BaseAddress, ClientId, ClientSecret);
        }
        public Task<long> GetRegisterCountAsync(UserRegisterCountRequest request)
        {
            return userService.RegisterCountAsync(request);
        }
        public Task<long> GetLoginCountAsync(LoginCountRequest request)
        {
            return loginService.LoginCountAsync(request);
        }
        public Task<int> GetUserOnlineCountAsync()
        {
            return loginService.UserOnlineCountAsync();
        }        
        public Task SetLastAccessAsync(Guid userId)
        {
            return loginService.SetLastAccessAsync(userId);
        }

        public async Task<ModelResult<LoginResponse>> LoginAsync(LoginRequest request)
        {
            //Microsoft.AspNetCore.Authorization.Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30));
            //System.Security.Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30));

            ModelResult<LoginResponse> modelResponse = new ModelResult<LoginResponse>();

            var response = await loginService.LoginAsync(request.LogonName, request.Password, this.TenantId);

            modelResponse.Messages.AddRange(response.Messages);
            if (response.Data != null)
            {
                modelResponse.Data = new LoginResponse()
                {
                    LogonName = response.Data.LogonName,
                    UserId = response.Data.UserId,
                    Name = response.Data.Name
                };
            }

            return modelResponse;
        }

        #region User Services

        public async Task<IEnumerable<UserRoleResponse>> GetEffectiveRolesAsync(UserRoleRequest request)
        {
            var response = await userService.GetEffectiveRolesAsync(request.UserId, request.ApplicationId);

            List<UserRoleResponse> responseModel = response.Select(p => new UserRoleResponse()
            {
                CodeName = p.CodeName
            }).ToList();

            return responseModel;
        }        

        public async Task<QueryResult<UserViewModel>> GetAsync(QueryRequest request)
        {
            var response = await userService.GetAsync(request);

            var result = QueryResult.Convert<UserViewModel>(response, response.Data.Select(p => GetUserViewModelFromDTO(p)));

            return result;
        }

        public async Task<bool> CheckEmailUsedAsync(string email)
        {
            var response = await userService.CheckEmailUsedAsync(email);
            return response;
        }
        public async Task<bool> CheckLogonNameUsedAsync(string email)
        {
            var response = await userService.CheckLogonNameUsedAsync(email);
            return response;
        }

        private UserViewModel GetUserViewModelFromDTO(UserViewModelDTO modelDTO)
        {
            return new UserViewModel()
            {
                BlockLogin = modelDTO.BlockLogin,
                Email = modelDTO.Email,
                LastNames = modelDTO.LastNames,
                LogonName = modelDTO.LogonName,
                Names = modelDTO.Names,
                UserId = modelDTO.UserId,
                UserGroupNames = modelDTO.UserGroupNames,
                IsValid = modelDTO.IsValid,
                FullName = modelDTO.FullName,
                LogonNameFullName = modelDTO.LogonNameFullName,
                Online = modelDTO.Online,
                LastAccess = modelDTO.LastAccess
            };
        }

        public async Task<UserViewModel> GetAsync(string logonName)
        {
            var data = await GetAsync(QueryRequest.Builder().AddCondition("LogonName",logonName));

            if(data.Data.Any())
                return data.Data.FirstOrDefault();                

            return null;            
        }

        public async Task<UserViewModel> GetAsync(Guid userId)
        {
            var model = await userService.GetAsync(userId);

            return GetUserViewModelFromDTO(model);
        }

        public async Task<ModelResult<UserCreateResponse>> CreateUserAsync(UserCreateRequest request)
        {
            ModelResult<UserCreateResponse> result = new ModelResult<UserCreateResponse>();

            var response = await userService.CreateAsync(new UserCreateRequestDTO()
            {
                BlockLogin = request.BlockLogin,
                Email = request.Email,
                LastNames = request.LastNames,
                LogonName = request.LogonName,
                Names = request.Names,
                Password = request.Password,
                UserId = request.UserId,
                RegisterDate = request.RegisterDate,
                Attributes = request.Attributes
            });

            result.AddMessages(response);

            if (result.Data != null)
                result.Data = new UserCreateResponse()
                {
                    UserId = response.Data.UserId
                };

            return result;
        }

        public async Task<ModelResult> UpdateUserAsync(UserUpdateRequest request)
        {
            var response = await userService.UpdateAsync(request.UserId, new UserUpdateRequestDTO()
            {
                BlockLogin = request.BlockLogin,
                Email = request.Email,
                LastNames = request.LastNames,
                LogonName = request.LogonName,
                Names = request.Names
            });

            return response;
        }
        public async Task<ModelResult> ChangePasswordAsync(Guid userId, string password)
        {
            var response = await userService.ChangePasswordAsync(userId, new UserChangePasswordRequestDTO()
            {
                Password = password
            });
            
            return response;
        }

        public async Task<ModelResult> DeleteUserAsync(UserDeleteRequest request)
        {
            var response = await userService.DeleteAsync(request.UserId);
            return response;
        }

        public async Task<IEnumerable<UserGroupDetailViewModel>> GetUserGroupForUserAsync(Guid userId)
        {
            var response = await userService.GetUserGroupsAsync(userId);

            return response.Select(p=> new UserGroupDetailViewModel()
            {
                Name = p.Name,
                UserGroupId = p.UserGroupId
            });
        }

        public async Task<ModelResult> AddUserGroupForUser(UserGroupAddToUserRequest request)
        {
             ModelResult result = new ModelResult();

            var response = await userService.AddUserGroupAsync(request.UserId, new UserAddUserGroupRequestDTO()
            {
                UserGroupId = request.UserGroupId
            });

            result.AddMessages(response);

            return result;
        }

        public async Task<ModelResult> RemoveUserGroupForUser(UserGroupRemoveToUserRequest request)
        {
             ModelResult result = new ModelResult();

            var response = await userService.RemoveUserGroupAsync(request.UserId, request.UserGroupId);

            result.AddMessages(response);

            return result;
        }

        public async Task<ModelResult> RemoveAllUserGroupForUser(Guid userId)
        {
             ModelResult result = new ModelResult();

            var response = await userService.RemoveAllUserGroupsAsync(userId);

            result.AddMessages(response);

            return result;
        }

        #endregion

        #region  User Group Services

        public async Task<QueryResult<UserGroupViewModel>> GetUserGroupsAsync(QueryRequest request)
        {
            var response = await userGroupService.GetAsync(request);

            var result = QueryResult.Convert<UserGroupViewModel>(response, response.Data.Select(p => new UserGroupViewModel()
            {
                Name = p.Name,
                UserGroupId = p.UserGroupId
            }));

            return result;
        }

        public async Task<UserGroupViewModel> GetUserGroupAsync(Guid userGroupId)
        {
            var model = await userGroupService.GetAsync(userGroupId);

            if (model != null)
                return new UserGroupViewModel()
                {
                    Name = model.Name,
                    UserGroupId = model.UserGroupId
                };
            
            return null;
        }

        public async Task<ModelResult<UserGroupCreateResponse>> CreateUserGroupAsync(UserGroupCreateRequest request)
        {
            ModelResult<UserGroupCreateResponse> result = new ModelResult<UserGroupCreateResponse>();

            var response = await userGroupService.CreateAsync(new UserGroupCreateRequestDTO()
            {
                Name = request.Name
            });

            result.AddMessages(response);

            if (result.Data != null)
                result.Data = new UserGroupCreateResponse()
                {
                    UserGroupId = response.Data.UserGroupId
                };

            return result;
        }

        public async Task<ModelResult> UpdateUserGroupAsync(UserGroupUpdateRequest request)
        {
            var response = await userGroupService.UpdateAsync(request.UserGroupId, new UserGroupUpdateRequestDTO()
            {
                Name = request.Name
            });

            return response;
        }

        public async Task<ModelResult> DeleteUserGroupAsync(UserGroupDeleteRequest request)
        {
            var response = await userGroupService.DeleteAsync(request.UserId);
            return response;
        }        

        #endregion

        #region 

        public async Task<ModelResult<OtpGenerateResponse>> GenerateOtpAsync(OtpGenerateRequest request)
        {
            var dataResult = await otpService.GenerateAsync(new OtpRequestDTO()
            {
                TargetId = request.TargetId,
                UserId = request.UserId,
                Challenge = request.Challenge
            });

            ModelResult<OtpGenerateResponse> response = new ModelResult<OtpGenerateResponse>();
            response.AddMessages(dataResult);
            
            if(dataResult.IsValid)
                response.Data = new OtpGenerateResponse()
                {
                    Password = dataResult.Data?.Password
                };

            return response;
        }        

        public async Task<ModelResult> ActivateOtpAsync(Guid userId, string targetId, string password)
        {
            var dataResult = await otpService.ActivateAsync(new OtpActivateRequestDTO()
            {
                Password = password,
                TargetId = targetId,
                UserId = userId
            });

            ModelResult response = new ModelResult();
            response.AddMessages(dataResult);
            
            return response;
        }

        public async Task<ModelResult> ActivateOtpAsync(string challenge, string targetId, string password)
        {
            var dataResult = await otpService.ActivateAsync(new OtpActivateRequestDTO()
            {
                Password = password,
                TargetId = targetId,
                Challenge = challenge
            });

            ModelResult response = new ModelResult();
            response.AddMessages(dataResult);
            
            return response;
        }

        #endregion

        public void Dispose()
        {
            this.httpClient.Dispose();
        }

    }
}
