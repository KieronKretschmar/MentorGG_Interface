using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Helpers
{
    public class RoleCreator
    {
        /// <summary>
        /// The names of the roles we use.
        /// </summary>
        public static readonly string[] RoleNames = { Subscriptions.Premium, Subscriptions.Ultimate };


        /// <summary>
        /// Create the ApplicationRoles, if not present.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void CreateRoles(IServiceProvider serviceProvider, string[] roleNames)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            foreach (var roleName in roleNames)
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    roleManager.CreateAsync(new ApplicationRole(roleName)).Wait();
                }
            }
        }
    }
}
