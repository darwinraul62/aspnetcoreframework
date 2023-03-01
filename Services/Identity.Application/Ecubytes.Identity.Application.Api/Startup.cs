using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Autofac;
using Ecubytes.DependencyInjection;
using Ecubytes.Identity.Application.Api.Infrastructure.AutofacModules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft;

namespace Ecubytes.Identity.Application.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddHostedService();

            services.AddControllersWithViews().
                AddDefaultJsonOptions();

            services.AddDefaultAuthenticationService(Configuration);

            services
            .AddDefaultAuthorizationPolicyProvider()
            .AddIdentityApplicationDbContext(Configuration)
            .AddDefaultLocalization(Configuration)
            .AddAwsSqsQueueService(Configuration)
            .AddDefaultEventBusService(Configuration)            
            .AddDefaultSwaggerGen(Configuration);
            

//  services.AddNewtonsoftJson(options => 
//         options.SerializerSettings.Converters.Add(new StringEnumConverter()));

// // order is vital, this *must* be called *after* AddNewtonsoftJson()
// services.AddSwaggerGenNewtonsoftSupport();

//             services.AddJsonOptions(options => 
//                 options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = ApiVersionReader.Combine(
                    //new HeaderApiVersionReader("api-version"),
                    new QueryStringApiVersionReader("version"));                    
            });
        }

         // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            builder.RegisterModule(new ApplicationModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            ServiceActivator.Configure(app.ApplicationServices);
            
            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger<Startup>().LogDebug("Using PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }
            
            app.UseSwagger()
               .UseSwaggerUI(c =>
               {                   
                   c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Ordering.API V1");
                   c.OAuthClientId("identityswaggerui");
                   c.OAuthAppName("Identity Swagger UI");
                                  
               });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDefaultLocalization();
           // app.UseDataBaseLogger(Configuration);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
