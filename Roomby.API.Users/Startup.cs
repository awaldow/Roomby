using System.Reflection;
using HealthChecks.UI.Client;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Roomby.API.Users.Extensions;

namespace Roomby.API.Users
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebHostEnv = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnv { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddApplicationInsightsTelemetry(options =>
            {
                options.ConnectionString = Configuration.GetSection("APPINSIGHTS_CONNECTIONSTRING").Value;
            });

            var domainAssembly = typeof(Startup).GetTypeInfo().Assembly;

            services.AddUsersMVC(Configuration)
                .AddUsersDbContext(Configuration, WebHostEnv)
                .AddSwagger(Configuration)
                .AddCustomHealthCheck(Configuration)
                .AddMediatR(domainAssembly)
                .AddFluentValidation(new[] { domainAssembly });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseSwaggerandSwaggerUI(Configuration, env, provider);
            app.UseSwaggerandRedoc(Configuration, env, provider);

             app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/live", new HealthCheckOptions()
                {
                    Predicate = r => r.Name.Contains("live")
                });
                endpoints.MapHealthChecks("/ready", new HealthCheckOptions()
                {
                    Predicate = r => r.Tags.Contains("services")
                });

                endpoints.MapControllers();
            });
        }
    }
}
