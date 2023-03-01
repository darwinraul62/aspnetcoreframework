using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ecubytes.Json;

namespace Microsoft.Extensions.DependencyInjection
{
    
    public static class MvcBuilderJsonOptionsExtensions
    {
        public static IMvcBuilder AddDefaultJsonOptions(this IMvcBuilder builder)
        {                     
            //Default
            return builder.AddDefaultJsonOptions(new Ecubytes.Json.JsonOptions());
        }

        public static IMvcBuilder AddDefaultJsonOptions(this IMvcBuilder builder, 
            Action<Ecubytes.Json.JsonOptions> configure)
        {
            Ecubytes.Json.JsonOptions options = new JsonOptions();
            configure(options);

            builder.AddDefaultJsonOptions(options);
            
            return builder;
        }

        public static IMvcBuilder AddDefaultJsonOptions(this IMvcBuilder builder,
            Ecubytes.Json.JsonOptions options)
        {
            builder.AddNewtonsoftJson();

            builder.AddJsonOptions(op => 
            {
                if(options.UseStringEnumConverter)
                    op.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                
                if(options.UseCamelCaseNamingPolicy)
                {
                    op.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;    
                    op.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;                 
                }

                JsonUtility.DefaultJsonSettings = op.JsonSerializerOptions;
            });
            
            return builder;
        }
    }
}
