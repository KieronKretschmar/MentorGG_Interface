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

namespace MentorInterface
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning();

            // LetsEncrypt
            services.AddLetsEncrypt();

            // Add Steam OpenID 2.0
            // https://github.com/aspnet-contrib/AspNet.Security.OpenId.Providers/tree/dev/src/AspNet.Security.OpenId.Steam
            // https://github.com/aspnet-contrib/AspNet.Security.OpenId.Providers/tree/dev/src/AspNet.Security.OpenId

            // Load the Authentication Section and confirm the entry exists.
            var steamApplicationKey = Configuration.GetSection("STEAM_API_KEY").Value;
            if(steamApplicationKey == null)
            {
                throw new ArgumentException("SteamApplicationKey is missing, configure the `STEAM_API_KEY` enviroment variable.");
            }

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })

            .AddCookie(options =>
            {
                options.LoginPath = "/authentication/signin";
                options.LogoutPath = "/authentication/signout";
            })

            .AddSteam(options =>
            {
                options.ApplicationKey = steamApplicationKey;
                options.Events.OnAuthenticated += AuthenticationHandler.HandleSuccess;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
