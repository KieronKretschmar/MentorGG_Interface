using Database;
using Entities.Models;
using Entities.Models.Paddle;
using MentorInterface.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Paddle
{
    /// <summary>
    /// Helper class for managing PaddlePlans.
    /// </summary>
    public static class PaddlePlanManager
    {
        /// <summary>
        /// List of all PaddlePlans.
        /// </summary>
        public static readonly List<PaddlePlanRoleBind> ProductionPlans = new List<PaddlePlanRoleBind>
        {
            // Premium
            new PaddlePlanRoleBind(new PaddlePlan(586570, Entities.SubscriptionType.Premium, 1, 6.99), new List<string> { RoleCreator.Premium.Name}),
            new PaddlePlanRoleBind(new PaddlePlan(586571, Entities.SubscriptionType.Premium, 3, 4.99), new List<string> { RoleCreator.Premium.Name}),
            new PaddlePlanRoleBind(new PaddlePlan(586572, Entities.SubscriptionType.Premium, 6, 4.59), new List<string> { RoleCreator.Premium.Name}),

            // Ultimate            
            new PaddlePlanRoleBind(new PaddlePlan(586580, Entities.SubscriptionType.Ultimate, 1, 16.99), new List<string> { RoleCreator.Ultimate.Name}),
            new PaddlePlanRoleBind(new PaddlePlan(586581, Entities.SubscriptionType.Ultimate, 3, 14.99), new List<string> { RoleCreator.Ultimate.Name}),
            new PaddlePlanRoleBind(new PaddlePlan(586582, Entities.SubscriptionType.Ultimate, 6, 14.59), new List<string> { RoleCreator.Ultimate.Name}),
        };


        /// <summary>
        /// Writes PaddlePlans and binds between PaddlePlans and ApplicationRoles to database for the Plans.
        /// Does not modify PaddlePlans or binds for plans already in the database.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void SetPaddlePlans(IServiceProvider serviceProvider, List<PaddlePlanRoleBind> binds)
        {
            var applicationContext = serviceProvider.GetRequiredService<ApplicationContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            foreach (var bind in binds)
            {
                var paddlePlan = bind.Plan;
                if (!applicationContext.PaddlePlan.Any(x => x.PlanId == paddlePlan.PlanId))
                {
                    // Create PaddlePlan
                    applicationContext.PaddlePlan.Add(paddlePlan);

                    // Create a PaddlePlanRole for each role this plan includes and write to database
                    foreach (var roleName in bind.RoleNames)
                    {
                        var roleId = roleManager.FindByNameAsync(roleName).Result.Id;
                        var paddlePlanRole = new PaddlePlanRole
                        {
                            PlanId = paddlePlan.PlanId,
                            RoleId = roleId,
                        };

                        applicationContext.PaddlePlanRole.Add(paddlePlanRole);
                    }
                }
            }
            applicationContext.SaveChanges();
        }
    }
}
