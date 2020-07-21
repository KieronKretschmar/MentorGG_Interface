using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using MentorInterface.Authentication;
using MentorInterface.Middleware;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Database;
using Prometheus;
using MentorInterface.Helpers;
using Microsoft.AspNetCore.Identity;
using Entities.Models.Paddle;
using MentorInterface.Paddle;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace MentorInterface
{
    /// <summary>
    /// Congifure the WebApi's enviroment.
    /// </summary>
    public class Startup
    {

        private bool IsDevelopment => Configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") == Environments.Development;

        /// <summary>
        /// The token which devs need to add in Headers["Authorization"]="Bearer <token>" to authenticate
        /// </summary>
        private string IDENTITY_WORKAROUND_BEARER_TOKEN => Configuration.GetValue<string>("IDENTITY_WORKAROUND_BEARER_TOKEN");

        /// <summary>
        /// The ApplicationUserId of the User assigned to devs who login with above Bearer Token.
        /// </summary>
        private string IDENTITY_WORKAROUND_USER_ID => Configuration.GetValue<string>("IDENTITY_WORKAROUND_USER_ID");

        /// <summary>
        /// Amount of times to attempt a successful MySQL connection on startup.
        /// </summary>
        const int MYSQL_RETRY_LIMIT = 3;

        /// <summary>
        /// Port to scrape metrics from at `/metrics`
        /// </summary>
        public const int METRICS_PORT = 9913;

        /// <summary>
        /// Extend and apply a supplied configuration.
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Current configuration.
        /// </summary>
        public IConfiguration Configuration { get; }


        /// <summary>
        /// Add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(x => 
                {
                    // Serialize JSON using the Member CASE!
                    x.UseMemberCasing();
                    // Serialize longs (steamIds) as strings
                    x.SerializerSettings.Converters.Add(new LongToStringConverter());
                });
            services.AddApiVersioning();

            #region Identity

            // Connect to the user database.
            var connString = Configuration.GetValue<string>("MYSQL_CONNECTION_STRING");
            if (connString != null)
            {
                services.AddDbContext<ApplicationContext>(o =>
                {
                    o.UseMySql(connString, sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(MYSQL_RETRY_LIMIT);
                    });

                }, ServiceLifetime.Scoped);
            }
            // Use InMemoryDatabase if the connectionstring is not set in a DEV enviroment
            else if (IsDevelopment)
            {
                services.AddDbContext<ApplicationContext>(o =>
                {
                    o.UseInMemoryDatabase("MyTemporaryDatabase");

                }, ServiceLifetime.Scoped);

                Console.WriteLine("WARNING: Using InMemoryDatabase!");
            }
            else
            {
                throw new ArgumentException(
                    "MySqlConnectionString is missing, configure the `MYSQL_CONNECTION_STRING` enviroment variable.");
            }
            
            if (Configuration.GetValue<bool>("IS_MIGRATING"))
            {
                Console.WriteLine("IS_MIGRATING is true! ARE YOU STILL MIGRATING?");
                return;
            }

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationContext>();

            #endregion

            #region Logging
            services.AddLogging(o =>
            {
                o.AddConsole(o =>
                {
                    o.TimestampFormat = "[yyyy-MM-dd HH:mm:ss zzz] ";
                });

                // Filter out ASP.Net and EFCore logs of LogLevel lower than LogLevel.Warning
                o.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
                o.AddFilter("Microsoft.EntityFrameworkCore.Infrastructure", LogLevel.Warning);
                o.AddFilter("Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker", LogLevel.Warning);
                o.AddFilter("Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor", LogLevel.Warning);
                o.AddFilter("Microsoft.AspNetCore.Hosting.Diagnostics", LogLevel.Warning);
                o.AddFilter("Microsoft.AspNetCore.Routing.EndpointMiddleware", LogLevel.Warning);

                // Filter logs for each forwarded request
                o.AddFilter("System.Net.Http.HttpClient", LogLevel.Warning);
            });
            #endregion

            #region Helpers
            services.AddTransient<IRoleHelper, RoleHelper>();
            services.AddTransient<ISteamUserOperator, SteamUserOperator>();
            #endregion

            #region Paddle & Subscriptions

            services.AddTransient<IWebhookVerifier, WebhookVerifier>(x =>
                {
                    return new WebhookVerifier(
                        File.ReadAllText("/app/Paddle/PaddlePublicKey.pem"));
                }
            );

            var PADDLE_VENDOR_ID = Configuration.GetValue<int>("PADDLE_VENDOR_ID");
            var PADDLE_VENDOR_AUTH_CODE = Configuration.GetValue<string>("PADDLE_VENDOR_AUTH_CODE")
                ?? throw new ArgumentNullException("The environment variable PADDLE_VENDOR_AUTH_CODE has not been set.");

            services.AddTransient<IPaddleApiCommunicator, PaddleApiCommunicator>(sp =>
            {
                return new PaddleApiCommunicator(sp.GetRequiredService<ILogger<PaddleApiCommunicator>>(), sp.GetRequiredService<IHttpClientFactory>(), PADDLE_VENDOR_ID, PADDLE_VENDOR_AUTH_CODE);
            });

            services.AddTransient<SubscriptionRemover>();
            services.AddHostedService<SubscriptionRemoverBackgroundService>();
            #endregion

            #region HTTP Clients

            // Add HTTP clients for communication with other services in the cluster
            services.AddConnectedHttpService(ConnectedServices.DemoCentral, Configuration, "DEMOCENTRAL_URL_OVERRIDE");
            services.AddConnectedHttpService(ConnectedServices.FaceitMatchGatherer, Configuration, "FACEITMATCHGATHERER_URL_OVERRIDE");
            services.AddConnectedHttpService(ConnectedServices.MatchRetriever, Configuration, "MATCHRETRIEVER_URL_OVERRIDE");
            services.AddConnectedHttpService(ConnectedServices.SharingCodeGatherer, Configuration, "SHARINGCODEGATHERER_URL_OVERRIDE");
            services.AddConnectedHttpService(ConnectedServices.SituationOperator, Configuration, "SITUATIONOPERATOR_URL_OVERRIDE");
            services.AddConnectedHttpService(ConnectedServices.SteamUserOperator, Configuration, "STEAMUSEROPERATOR_URL_OVERRIDE");

            // Add HTTP clients for external communication
            services.AddConnectedHttpService(ConnectedServices.PaddleApi, Configuration, "PADDLEAPI_URL_OVERRIDE");
            #endregion

            #region Authentication

            var steamApplicationKey = Configuration.GetValue<string>("STEAM_API_KEY")
                ?? throw new ArgumentNullException("The environment variable STEAM_API_KEY has not been set.");

            var jwtSigningKey = Configuration.GetValue<string>("JWT_SIGNING_KEY")
                ?? throw new ArgumentNullException("The environment variable JWT_SIGNING_KEY has not been set.");

            var jwtIssuer = Configuration.GetValue<string>("JWT_ISSUER")
                ?? throw new ArgumentNullException("The environment variable JWT_ISSUER has not been set.");

            var jwtAudience = Configuration.GetValue<string>("JWT_AUDIENCE")
                ?? throw new ArgumentNullException("The environment variable JWT_AUDIENCE has not been set.");

            var jwtValidity = Configuration.GetValue<int?>("JWT_VALIDITY")
                ?? throw new ArgumentNullException("The environment variable JWT_VALIDITY has not been set.");

            services.AddSingleton<IJsonWebTokenGenerator>(sp =>
                {
                    return new JsonWebTokenGenerator(
                        jwtSigningKey,
                        jwtIssuer,
                        jwtAudience,
                        TimeSpan.FromMinutes(jwtValidity),
                        sp.GetRequiredService<ILogger<JsonWebTokenGenerator>>());
                }
            );

            services
                .AddAuthentication(options => 
                {
                    options.DefaultAuthenticateScheme = MentorAuthenticationSchemes.JWT;
                    options.DefaultChallengeScheme = MentorAuthenticationSchemes.JWT;
                })
                .AddJwtBearer(MentorAuthenticationSchemes.JWT, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,  
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKey)) 
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            // Add the access_token as a claim, as we may need it
                            var accessToken = context.SecurityToken as JwtSecurityToken;
                            if (accessToken != null)
                            {
                                ClaimsIdentity identity = context.Principal.Identity as ClaimsIdentity;
                                if (identity != null)
                                {
                                    identity.AddClaim(new Claim("access_token", accessToken.RawData));
                                }
                            }

                            return Task.CompletedTask;
                        }
                    };


                })
                .AddSteam(MentorAuthenticationSchemes.STEAM, options =>
                {
                    options.ApplicationKey = steamApplicationKey;
                    options.CallbackPath = "/openid/callback/steam";

                    options.Events = new AspNet.Security.OpenId.OpenIdAuthenticationEvents
                    {
                        OnAuthenticated = context =>
                        {
                            // Add the SteamId as a claim to isolate the OpenId Claim logic
                            ClaimsIdentity identity = context.Identity as ClaimsIdentity;
                            if (identity != null)
                            {
                                Claim steamCommunityClaim = identity.Claims.Single(o =>
                                {
                                    return o.Value.Contains("openid/id");
                                });
                                
                                var success = long.TryParse(steamCommunityClaim.Value.Split('/').Last(), out long steamId);
                                if (success)
                                {
                                    identity.AddClaim(new Claim("steamId", steamId.ToString()));
                                }
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            
            services.AddAuthorization(options =>
            {
                    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                        MentorAuthenticationSchemes.JWT);

                    defaultAuthorizationPolicyBuilder = 
                        defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();

                    options.AddPolicy("Steam", o => 
                    {
                        o.AddAuthenticationSchemes(MentorAuthenticationSchemes.STEAM);
                        o.RequireAuthenticatedUser();
                    });
            });

            // This enable more verbose logging and should be removed in Production
            // Todo Remove this
            IdentityModelEventSource.ShowPII = true;

            #endregion

            #region Cors
            services.AddCors(o => o.AddPolicy("Debug", builder =>
            {
                var allowedOrigins = new string[]
                {
                    "http://localhost:8080",
                    "https://localhost:8080",
                };
                builder.WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));
            #endregion

            #region Swagger
            services.AddSwaggerGen(options =>
            {
                OpenApiInfo interface_info = new OpenApiInfo { Title = "Mentor Interface", Version = "v1", };
                options.SwaggerDoc("v1", interface_info);

                // Generate documentation based on the Docstrings provided.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.EnableAnnotations();
            });
            #endregion

        }

        /// <summary>
        /// Configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (IsDevelopment)
            {
                app.UseCors("Debug");
            }

	    app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            #region Swagger
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "swagger";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Mentor Interface");
            });
            #endregion

            app.UseRouting();

            // Who is the user?
            app.UseAuthentication();

            // Configure the app to check for an authorization header with the configured bearer token
            // If it is present, authenticate as the user with the configured User Id
            // The entire app.Use() statement could be removed while keeping the Cookie-based login intact
            app.Use(async (context, next) =>
            {
                // Check if authorization header for workaround login is available
                if(!context.User.Identity.IsAuthenticated && IDENTITY_WORKAROUND_BEARER_TOKEN != null && context.Request.Headers["Authorization"] == "Bearer " + IDENTITY_WORKAROUND_BEARER_TOKEN)
                {
                    var principal = new ClaimsPrincipal();

                    // Determine Application User Id
                    // Authenticate as user specified by header or use default value instead
                    string appUserId = long.TryParse(context.Request.Headers["Impersonate-ApplicationUserId"], out var applicationUserId) 
                        ? applicationUserId.ToString()
                        : IDENTITY_WORKAROUND_USER_ID;

                    context.User = principal;

                    // Assign the current user the identity of the user with the User Id IDENTITY_WORKAROUND_USER_ID
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, appUserId));
                    var claimsIdentity = new ClaimsIdentity(claims, "Identity.Application");
                    principal.AddIdentity(claimsIdentity);
                }

                await next();
            });

            // Is the user allowed to perform this action?
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMetricServer(METRICS_PORT);

            // Migrate if this is not an inmemory database
            if (serviceProvider.GetRequiredService<ApplicationContext>().Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                serviceProvider.GetRequiredService<ApplicationContext>().Database.Migrate();
            }

            SeedDatabase(serviceProvider);
        }

        /// <summary>
        /// Seed the Database with Role information.
        /// </summary>
        /// <param name="serviceProvider"></param>
        private void SeedDatabase(IServiceProvider serviceProvider)
        {
            RoleCreator.CreateRoles(serviceProvider, RoleCreator.ApplicationRoles);

            // Write PaddlePlans to db and connect them with Roles
            var plans = PaddlePlanManager.ProductionPlans;
            PaddlePlanManager.SetPaddlePlans(serviceProvider, plans);
        }
    }
}
