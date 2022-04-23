using JHipsterNet.Web.Pagination.Binders;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ocelot.Middleware;

namespace Jhipster.Configuration
{
    public static class WebConfiguration
    {
        public static IServiceCollection AddWebModule(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddMvc();

            //https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-2.2
            services.AddHealthChecks();

            services.AddControllers(options => { options.ModelBinderProviders.Insert(0, new PageableBinderProvider()); })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            return services;
        }

        public static IApplicationBuilder UseApplicationWeb(this IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Map("/services", MapApiGateway);

            app.UseHealthChecks("/health");

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
            });
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            return app;
        }

        private static void MapApiGateway(IApplicationBuilder app)
        {
            app.UseOcelot().Wait();
        }
    }
}
