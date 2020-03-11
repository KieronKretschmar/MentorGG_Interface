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

namespace MentorInterface
{
    /// <summary>
    /// Congifure the WebApi's enviroment.
    /// </summary>
    public class Startup
    {

        private bool IsDevelopment => Configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") == Environments.Development;


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

            #region Helpers
            services.AddTransient<IRoleHelper, RoleHelper>();
            #endregion

            #region Paddle

            services.AddTransient<IWebhookVerifier, WebhookVerifier>(x =>
                {
                    return new WebhookVerifier(
                        File.ReadAllText("Paddle/PaddlePublicKey.pem"));
                }
            );

            #endregion

            #region HTTP Clients

            // Add HTTP clients with potentially overriden urls.
            services.AddConnectedHttpService(ConnectedServices.DemoCentral, Configuration, "DEMOCENTRAL_URL_OVERRIDE");
            services.AddConnectedHttpService(ConnectedServices.FaceitMatchGatherer, Configuration, "FACEITMATCHGATHERER_URL_OVERRIDE");
            services.AddConnectedHttpService(ConnectedServices.MatchRetriever, Configuration, "MATCHRETRIEVER_URL_OVERRIDE");
            services.AddConnectedHttpService(ConnectedServices.SharingCodeGatherer, Configuration, "SHARINGCODEGATHERER_URL_OVERRIDE");

            #endregion

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


            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationContext>();

            #endregion

            #region Application Cookie
            services
                .ConfigureApplicationCookie(options =>
                {
                    options.Cookie.Name = "MentorInterface.Identity";
                    options.LoginPath = "/authentication/signin/steam";
                    options.LogoutPath = "/authentication/signout/steam";

                    // Return 401 OR 403 instead of redirecting to LoginPath
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = async (context) => {
                            context.Response.StatusCode = 401;
                            await Task.CompletedTask;
                        },

                        OnRedirectToAccessDenied = async (context) => {
                            context.Response.StatusCode = 403;
                            await Task.CompletedTask;
                        },
                    };
                });
            #endregion

            #region Steam Authentication
            // Load the Authentication Section and confirm the entry exists.
            var steamApplicationKey = Configuration.GetValue<string>("STEAM_API_KEY");
            if (steamApplicationKey != null)
            {
                services
                    .AddAuthentication()
                    .AddSteam(scheme: MentorAuthenticationSchemes.STEAM, options =>
                    {
                        options.ApplicationKey = steamApplicationKey;
                        options.CallbackPath = "/openid/callback/steam";
                    });
            }
            // If in a DEV context, warn, dont throw.
            else if (IsDevelopment)
            {
                Console.WriteLine("WARNING: `STEAM_API_KEY` is missing!");
            }
            else
            {
                throw new ArgumentException("SteamApplicationKey is missing, configure the `STEAM_API_KEY` enviroment variable.");
            }
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

            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            if (IsDevelopment)
            {
                app.UseCors("Debug");
            }

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
