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
using AspNet.Security.OpenId;
using MentorInterface.Authentication;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Identity;
using MentorInterface.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MentorInterface
{
    /// <summary>
    /// Congifure the WebApi's enviroment.
    /// </summary>
    public class Startup
    {
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
            services.AddControllers();
            services.AddApiVersioning();
            services.AddLetsEncrypt();

            #region Identity
            var mySqlConnectionString = Configuration.GetSection("USER_DB_CONNECTION").Value;
            if (mySqlConnectionString == null)
            {
                throw new ArgumentException("MySqlConnectionString is missing, configure the `USER_DB_CONNECTION` enviroment variable.");
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseMySql(mySqlConnectionString);
                });
            }

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services
                .ConfigureApplicationCookie(options =>
                {
                    options.Cookie.Name = "MentorInterface.Identity";
                    options.LoginPath = "/authentication/signin/steam";
                    options.LogoutPath = "/authentication/signout/steam";

                    // Fired when a user successfully proves their Identity when accessing a protected resource
                    options.Events.OnValidatePrincipal += AuthenticationHandler.OnValidated;
                });
            #endregion

            #region Steam Authentication
            // https://github.com/aspnet-contrib/AspNet.Security.OpenId.Providers/tree/dev/src/AspNet.Security.OpenId.Steam
            // https://github.com/aspnet-contrib/AspNet.Security.OpenId.Providers/tree/dev/src/AspNet.Security.OpenId

            // Load the Authentication Section and confirm the entry exists.
            var steamApplicationKey = Configuration.GetSection("STEAM_API_KEY").Value;
            if(steamApplicationKey == null)
            {
                throw new ArgumentException("SteamApplicationKey is missing, configure the `STEAM_API_KEY` enviroment variable.");
            }

            services
                .AddAuthentication(defaultScheme: MentorAuthenticationSchemes.STEAM)
                .AddSteam(scheme: MentorAuthenticationSchemes.STEAM, options =>
                {
                    options.ApplicationKey = steamApplicationKey;
                    options.CallbackPath = "/openid/callback/steam";
                });
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
            });
            #endregion

        }

        /// <summary>
        /// Configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            #region Swagger
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
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

        }
    }
}
