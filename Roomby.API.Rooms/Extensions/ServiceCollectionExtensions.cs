using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Roomby.API.Rooms;
using Roomby.API.Rooms.Data;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rooms.API.Rooms.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static Task AuthenticationFailed(AuthenticationFailedContext arg)
        {
            var s = $"AuthenticationFailed: {arg.Exception.Message}";
            arg.Response.ContentLength = s.Length;
            arg.Response.Body.Write(Encoding.UTF8.GetBytes(s), 0, s.Length);
            return Task.FromResult(0);
        }

        public static IServiceCollection AddRoomsMVC(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.Authority = $"https://login.microsoftonline.com/tfp/{configuration["AzureAdB2C:TenantId"]}/{configuration["AzureAdB2C:Policy"]}/v2.0/";
                    jwtOptions.Audience = configuration["AzureAdB2C:ClientId"];
                    jwtOptions.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = AuthenticationFailed
                    };
                });

            services.AddMvc(options =>
            {
                //options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
            .AddNewtonsoftJson(x =>
            {
                x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                x.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            })
            .AddControllersAsServices();

            services.AddApiVersioning(
               options =>
               {
                   // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                   options.ReportApiVersions = true;
               });
            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("ReadScope", policy => policy.Requirements.Add(new ReadScope(configuration["AzureAdB2C:ScopeRead"])));
            //    options.AddPolicy("WriteScope", policy => policy.Requirements.Add(new WriteScope(configuration["AzureAdB2C:ScopeWrite"])));
            //});

            //services.AddSingleton<IAuthorizationHandler, ReadScopeHandler>();
            //services.AddSingleton<IAuthorizationHandler, WriteScopeHandler>();

            return services;
        }

        public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            //var accountName = configuration.GetValue<string>("AzureStorageAccountName");
            //var accountKey = configuration.GetValue<string>("AzureStorageAccountKey");

            var hcBuilder = services.AddHealthChecks();
            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy())
                    .AddSqlServer(
                            connectionString: configuration.GetConnectionString("RoombyRoomSql"),
                            name: "roomDB-check",
                            tags: new string[] { "services" });

            //if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
            //{
            //    hcBuilder
            //        .AddAzureServiceBusQueue(
            //            configuration["EventBusConnection"],
            //            queueName: "organization_event_bus",
            //            name: "org-servicebus-check",
            //            tags: new string[] { "servicebus" });
            //}
            //else
            //{
            //    hcBuilder
            //        .AddRabbitMQ(
            //            $"amqp://{configuration["EventBusConnection"]}",
            //            name: "org-rabbitmqbus-check",
            //            tags: new string[] { "rabbitmqbus" });
            //}

            //hcBuilder.AddApplicationInsightsPublisher(configuration["ApplicationInsights:InstrumentationKey"]);

            //services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService), typeof(HealthCheckPublisherOptions).Assembly.GetType("Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckPublisherHostedService")));
            hcBuilder.AddApplicationInsightsPublisher();
            return services;
        }

        public static IServiceCollection AddItemsDbContext(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddDbContext<RoomsContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("RoombyRoomSql"), sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });

                    // TODO: Limit this to dev and staging
                    if (env.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                }
            });


            //services.AddDbContext<IntegrationEventLogContext>(options =>
            //{
            //    options.UseSqlServer(configuration["InventoryDb"],
            //                         sqlServerOptionsAction: sqlOptions =>
            //                         {
            //                             sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
            //                                 //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
            //                                 sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            //                         });
            //});

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();
            services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }

        public static IServiceCollection AddItemsOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RoomsSettings>(configuration);
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Please refer to the errors property for additional details."
                    };

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });

            return services;
        }

        //public static IServiceCollection AddIntegrationServices(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
        //        sp => (DbConnection c) => new IntegrationEventLogService(c));
        //    services.AddTransient<IOrganizationEventService, OrganizationEventService>();
        //    if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
        //    {
        //        services.AddSingleton<IServiceBusPersistentConnection>(sp =>
        //        {
        //            var settings = sp.GetRequiredService<IOptions<OrganizationSettings>>().Value;
        //            var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersistentConnection>>();
        //            var serviceBusConnection = new ServiceBusConnectionStringBuilder(settings.EventBusConnection + "EntityPath=organization_event_bus");
        //            return new DefaultServiceBusPersistentConnection(serviceBusConnection, logger);
        //        });
        //    }
        //    else
        //    {
        //        services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
        //        {
        //            var settings = sp.GetRequiredService<IOptions<OrganizationSettings>>().Value;
        //            var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

        //            var factory = new ConnectionFactory()
        //            {
        //                HostName = configuration["EventBusConnection"],
        //                DispatchConsumersAsync = true
        //            };

        //            if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
        //            {
        //                factory.UserName = configuration["EventBusUserName"];
        //            }

        //            if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
        //            {
        //                factory.Password = configuration["EventBusPassword"];
        //            }

        //            var retryCount = 5;
        //            if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
        //            {
        //                retryCount = int.Parse(configuration["EventBusRetryCount"]);
        //            }

        //            return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
        //        });
        //    }

        //    return services;
        //}

        //public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var subscriptionClientName = configuration["SubscriptionClientName"];

        //    if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
        //    {
        //        services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
        //        {
        //            var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersistentConnection>();
        //            var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
        //            var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
        //            var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

        //            return new EventBusServiceBus(serviceBusPersisterConnection, logger,
        //                eventBusSubcriptionsManager, subscriptionClientName, iLifetimeScope);
        //        });

        //    }
        //    else
        //    {
        //        services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
        //        {
        //            var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
        //            var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
        //            var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
        //            var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

        //            var retryCount = 5;
        //            if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
        //            {
        //                retryCount = int.Parse(configuration["EventBusRetryCount"]);
        //            }

        //            return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
        //        });
        //    }

        //    services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        //    //services.AddTransient<OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
        //    //services.AddTransient<OrderStatusChangedToPaidIntegrationEventHandler>();

        //    return services;
        //}
    }
}
