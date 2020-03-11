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
    public static class PaddlePlanManager
    {
        /// <summary>
        /// List of all PaddlePlans.
        /// </summary>
        public static readonly List<PaddlePlan> ProductionPlans = new List<PaddlePlan>
        {
            // Premium            
            new PaddlePlan(586570, Entities.SubscriptionType.Premium, 1, 6.99),
            new PaddlePlan(586571, Entities.SubscriptionType.Premium, 3, 4.99),
            new PaddlePlan(586572, Entities.SubscriptionType.Premium, 6, 4.49),

            // Ultimate            
            new PaddlePlan(586580, Entities.SubscriptionType.Ultimate, 1, 16.99),
            new PaddlePlan(586581, Entities.SubscriptionType.Ultimate, 3, 14.99),
            new PaddlePlan(586582, Entities.SubscriptionType.Ultimate, 6, 14.49),
                
        };


        /// <summary>
        /// Writes PaddlePlans and binds between PaddlePlans and ApplicationRoles to database for the Plans.
        /// Does not modify PaddlePlans or binds for plans already in the database.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void SetPaddlePlans(IServiceProvider serviceProvider, List<PaddlePlan> plans)
        {
            var applicationContext = serviceProvider.GetRequiredService<ApplicationContext>();
            var roleMananger = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            foreach (var paddlePlan in plans)
            {
                if (!applicationContext.PaddlePlan.Any(x => x.PlanId == paddlePlan.PlanId))
                {
                    // Create PaddlePlan
                    applicationContext.PaddlePlan.Add(paddlePlan);

                    // Create a PaddlePlanRole for each role this plan includes and write to database
                    foreach (var roleName in Subscriptions.GetSubscription(paddlePlan.SubscriptionType).Roles)
                    {
                        var roleId = roleMananger.FindByNameAsync(roleName).Result.Id;
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
