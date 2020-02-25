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
        /// List of all PaddlePlans and the roles they enable.
        /// </summary>
        public static readonly List<PaddlePlanRoleBind> ProductionBinds = new List<PaddlePlanRoleBind>
        {
            new PaddlePlanRoleBind(583755, new List<string> {Subscriptions.Premium }),
            new PaddlePlanRoleBind(583756, new List<string> {Subscriptions.Ultimate }),
        };
        

        /// <summary>
        /// Writes PaddlePlans and binds between PaddlePlans and ApplicationRoles to database for the Plans.
        /// Does not modify PaddlePlans or binds for plans already in the database.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void SetPaddlePlans(IServiceProvider serviceProvider, List<PaddlePlanRoleBind> binds)
        {
            var applicationContext = serviceProvider.GetRequiredService<ApplicationContext>();
            var roleMananger = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            foreach (var roleBind in binds)
            {
                if (!applicationContext.PaddlePlan.Any(x => x.PlanId == roleBind.PlanId))
                {
                    // Create PaddlePlan
                    var paddlePlan = new PaddlePlan { PlanId = roleBind.PlanId };
                    applicationContext.PaddlePlan.Add(paddlePlan);

                    // Create a PaddlePlanRole for each role this plan includes and write to database
                    foreach (var roleName in roleBind.RoleNames)
                    {
                        var roleId = roleMananger.FindByNameAsync(roleName).Result.Id;
                        var paddlePlanRole = new PaddlePlanRole
                        {
                            PlanId = roleBind.PlanId,
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
