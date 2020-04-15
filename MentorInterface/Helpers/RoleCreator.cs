using Entities;
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
        /// The roles we use.
        /// </summary>
        public static ApplicationRole[] ApplicationRoles => new ApplicationRole[] { Premium, Ultimate };

        /// <summary>
        /// Initial value for Premium role
        /// </summary>
        public static ApplicationRole Premium = new ApplicationRole(SubscriptionType.Premium.ToString(), 100);

        /// <summary>
        /// Initial value for Ultimate role
        /// </summary>
        public static ApplicationRole Ultimate = new ApplicationRole(SubscriptionType.Ultimate.ToString(), 100);

        /// <summary>
        /// Create the ApplicationRoles, if not present.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void CreateRoles(IServiceProvider serviceProvider, ApplicationRole[] roles)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role.Name).Result)
                {
                    roleManager.CreateAsync(role).Wait();
                }
            }
        }
    }
}
