using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Roomby.API.Rooms.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerandRedoc(this IApplicationBuilder app, IConfiguration config, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger(c =>
            {
                var basePath = env.IsDevelopment() ? "/" : "/";
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    //swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{basePath}" } };
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"https://{httpReq.Host.Value}{basePath}" } };
                });
            });
            app.UseReDoc(c =>
            {
                var baseUrl = config["ApiBaseUrl"];
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    if (!env.IsDevelopment())
                    {
                        c.SpecUrl($"{baseUrl}/swagger/{desc.GroupName}/swagger.json");
                    }
                    else
                    {
                        c.SpecUrl($"/swagger/{desc.GroupName}/swagger.json");
                    }

                }
            });
            return app;
        }

        public static IApplicationBuilder UseSwaggerandSwaggerUI(this IApplicationBuilder app, IConfiguration config, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger(c =>
            {
                var basePath = env.IsDevelopment() ? "/" : "/";
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    //swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{basePath}" } };
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"https://{httpReq.Host.Value}{basePath}" } };
                });
            });
            app.UseSwaggerUI(c =>
            {
                // May do versioning here, we'll see how we go
                c.OAuthClientId(config["AzureAd:ClientId"]);
                c.OAuthScopeSeparator(" ");
                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                c.OAuthAppName("CMSService.API");
                c.DocExpansion(DocExpansion.None);
                var baseUrl = config["ApiBaseUrl"];
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    if (!env.IsDevelopment())
                    {
                        c.SwaggerEndpoint($"{baseUrl}/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
                    }
                    else
                    {
                        c.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
                    }

                }
            });
            return app;
        }
    }
}