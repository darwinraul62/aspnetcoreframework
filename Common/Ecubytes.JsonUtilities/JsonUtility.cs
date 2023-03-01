using System;
using System.Text.Json;
using Ecubytes.Data;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;

namespace Ecubytes.Json
{
    public static class JsonUtility
    {
        public static JsonSerializerOptions DefaultJsonSettings { get; set; }

        public static T Deserialize<T>(string model)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(model, DefaultJsonSettings);
        }
        public static string Serialize(object model)
        {
            return System.Text.Json.JsonSerializer.Serialize(model);
        }

        public static string Serialize<T>(JsonPatchDocument<T> model) where T : class
        {
            return JsonConvert.SerializeObject(model);
        }

        public static ModelResult DeserializeAsModelResult(string model)
        {
            return JsonUtility.Deserialize<ModelResult>(model);
        }
        public static ModelResult<T> DeserializeAsModelResult<T>(string model)
        {
            return JsonUtility.Deserialize<ModelResult<T>>(model);
        }

    }
}
