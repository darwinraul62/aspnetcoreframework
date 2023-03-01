using System;
using System.Text.Json;
using System.Threading.Tasks;
using Ecubytes.Data;
using Ecubytes.Json;

namespace System.Net.Http
{
    public static class HttpContentExtensions
    {            
        public static async Task<T> ReadAsAsync<T>(this HttpContent httpContent)
        {
            return JsonUtility.Deserialize<T>(await httpContent.ReadAsStringAsync());
        }
        public static async Task<ModelResult> ReadAsModelResultAsync(this HttpContent httpContent)
        {
            return JsonUtility.DeserializeAsModelResult(await httpContent.ReadAsStringAsync());
        }
        public static async Task<ModelResult<T>> ReadAsModelResultAsync<T>(this HttpContent httpContent)
        {
            return JsonUtility.DeserializeAsModelResult<T>(await httpContent.ReadAsStringAsync());
        }
    }
}
