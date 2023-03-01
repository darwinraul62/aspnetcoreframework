using System;
using System.Net.Http;
using System.Threading.Tasks;
using Ecubytes.Data;
using Ecubytes.AspNetCore.Http;
using System.Text.Json;
using Ecubytes.AspNetCore.WebUtilities.Api;
using Ecubytes;
using Ecubytes.Json;
using Microsoft.AspNetCore.JsonPatch;

namespace System.Net.Http
{
    public static class HttpClientExtensions
    {
        private static StringContent GetStringJsonContent(object model)
        {
            return new StringContent(JsonUtility.Serialize(model), System.Text.Encoding.UTF8, "application/json");
        }

        private static StringContent GetStringJsonContent<T>(JsonPatchDocument<T> model) where T : class
        {
            return new StringContent(JsonUtility.Serialize(model), System.Text.Encoding.UTF8, "application/json-patch+json");
        }

        public static void SetProfile(this HttpClient httpClient, string profileName)
        {
            using (var services = Ecubytes.DependencyInjection.ServiceActivator.GetScope())
            {
                ApiProfileManager profileManager = (ApiProfileManager)services.ServiceProvider.GetService(typeof(ApiProfileManager));

                httpClient.BaseAddress = new Uri(profileManager.GetBaseAddress(profileName));
            }
        }

        public static async Task<T> GetAsync<T>(this HttpClient httpClient, string uri, bool returnDefaultOnNotFound = true)
        {
            try
            {
                string response = await httpClient.GetStringAsync(uri);
                return JsonUtility.Deserialize<T>(response);
            }   
            catch(HttpRequestException ex)
            {                
                if(returnDefaultOnNotFound && ex.StatusCode == HttpStatusCode.NotFound)
                    return default(T);
                else
                    throw ex;
            }                     
        }

        public static async Task<ModelResult<T>> GetModelResultAsync<T>(this HttpClient httpClient, string uri)
        {
            string response = await httpClient.GetStringAsync(uri);
            return JsonUtility.DeserializeAsModelResult<T>(response);
        }

        public static async Task<ModelResult> GetModelResultAsync(this HttpClient httpClient, string uri)
        {
            string response = await httpClient.GetStringAsync(uri);
            return JsonUtility.DeserializeAsModelResult(response);
        }

        public static async Task<HttpResponseMessageModel<T>> PostAsJsonAsync<T>(this HttpClient httpClient,
            string uri, object model)
        {
            HttpResponseMessageModel<T> result = new HttpResponseMessageModel<T>();
            var response = await httpClient.PostAsync(uri, GetStringJsonContent(model));
            result.HttpMessage = response;

            if (response.IsSuccessStatusCode)
                result.Data = JsonUtility.Deserialize<T>(await response.Content.ReadAsStringAsync());

            return result;
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsync(this HttpClient httpClient, string uri, object model)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            result = await httpClient.PostAsync(uri, GetStringJsonContent(model));
            return result;
        }

        public static async Task<HttpResponseMessage> PutAsJsonAsync(this HttpClient httpClient, string uri, object model)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            result = await httpClient.PutAsync(uri, GetStringJsonContent(model));
            return result;
        }
        public static async Task<HttpResponseMessage> PacthAsJsonAsync<T>(this HttpClient httpClient, string uri, JsonPatchDocument<T> patchDocument) where T : class
        {
            HttpResponseMessage result = new HttpResponseMessage();
            result = await httpClient.PatchAsync(uri, GetStringJsonContent(patchDocument));
            return result;
        }
    }
}
