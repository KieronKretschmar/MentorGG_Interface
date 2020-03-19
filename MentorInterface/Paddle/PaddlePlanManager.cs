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
        /// WARNING: When changing or replacing these, make sure there's no interference with old plans from database.
        /// </summary>
        public static readonly List<PaddlePlanRoleBind> ProductionPlans = new List<PaddlePlanRoleBind>
        {
            // Premium
            new PaddlePlanRoleBind(new PaddlePlan(587490, Entities.SubscriptionType.Premium, 1, 7.49), new List<string> { RoleCreator.Premium.Name}),
            new PaddlePlanRoleBind(new PaddlePlan(587491, Entities.SubscriptionType.Premium, 3, 5.99), new List<string> { RoleCreator.Premium.Name}),
            new PaddlePlanRoleBind(new PaddlePlan(587493, Entities.SubscriptionType.Premium, 6, 5.49), new List<string> { RoleCreator.Premium.Name}),

            // Ultimate            
            new PaddlePlanRoleBind(new PaddlePlan(587494, Entities.SubscriptionType.Ultimate, 1, 12.99), new List<string> { RoleCreator.Ultimate.Name}),
            new PaddlePlanRoleBind(new PaddlePlan(587495, Entities.SubscriptionType.Ultimate, 3, 10.99), new List<string> { RoleCreator.Ultimate.Name}),
            new PaddlePlanRoleBind(new PaddlePlan(587496, Entities.SubscriptionType.Ultimate, 6, 9.49), new List<string> { RoleCreator.Ultimate.Name}),
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
