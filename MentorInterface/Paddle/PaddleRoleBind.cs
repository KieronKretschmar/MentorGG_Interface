using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Paddle
{
    /// <summary>
    /// Paddle Role Bind
    /// Bind a PaddlePlanId to Application Roles.
    /// </summary>
    public struct PaddlePlanRoleBind
    {
        public readonly int PlanId;

        public readonly List<string> RoleNames;

        /// <summary>
        /// Create a RoleBind
        /// </summary>
        public PaddlePlanRoleBind(int planId, List<string> roleNames)
        {
            PlanId = planId;
            RoleNames = roleNames;
        }
    }
}
