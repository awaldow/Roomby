using System;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.Configuration;
using Roomby.API.Rooms.Extensions;

namespace Roomby.API.Rooms
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;
        readonly IConfiguration config;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration)
        {
            this.provider = provider;
            config = configuration;
        }

        public void Configure(SwaggerGenOptions options)
        {
            //options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            //{
            //    Type = SecuritySchemeType.OAuth2,
            //    Description = "OAuth2 Authentication Grant",
            //    Flows = new OpenApiOAuthFlows
            //    {
            //        Implicit = new OpenApiOAuthFlow
            //        {
            //            AuthorizationUrl = new Uri($"https://login.microsoftonline.com/{config["AzureAdB2C:TenantName"]}/oauth2/v2.0/authorize?p={config["AzureAdB2C:Policy"]}&response_mode=fragment", UriKind.Absolute),
            //            Scopes = new Dictionary<string, string>
            //                {
            //                    { "openid", "Open ID" },
            //                    { $"https://{config["AzureAdB2C:TenantName"]}/{config["AzureAdB2C:AppIDURI"]}/{config["AzureAdB2C:ScopeRead"]}", "Access read operations" },
            //                    { $"https://{config["AzureAdB2C:TenantName"]}/{config["AzureAdB2C:AppIDURI"]}/{config["AzureAdB2C:ScopeWrite"]}", "Access write operations" }
            //                }
            //        }
            //    }
            //});
            //options.AddSecurityRequirement(new OpenApiSecurityRequirement
            //    {
            //        {
            //            new OpenApiSecurityScheme
            //            {
            //                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            //            },
            //            new[] { "read-access", "write-access" }
            //        }
            //    });
            options.SchemaFilter<CustomNameSchemaFilter>();
            //options.OperationFilter<OperationIdConventionFilter>();
            
            var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
            options.IncludeXmlComments(xmlCommentsFullPath);
            options.DescribeAllEnumsAsStrings();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Roomby Rooms API",
                Version = description.ApiVersion.ToString(),
                Description = "API for Roomby Rooms - Test",
                Contact = new OpenApiContact()
                {
                    Email = "a.wal.bear@gmail.com",
                    Name = "Addison Waldow",
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API has been deprecated.";
            }
            return info;
        }
    }
}