using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public static class ApplicationContextExtensions
    {
        /// <summary>
        /// Return an ApplicationRole for a bound PaddlePlan
        /// </summary>
        public static async Task<ApplicationRole> RoleFromPaddlePlanIdAsync(
            this ApplicationContext applicationContext, int planId)
        {
            return await applicationContext.PaddlePlan
                .Where(x => x.PlanId == planId)
                .Select(x => x.Role)
                .SingleAsync();
        }
    }
}
