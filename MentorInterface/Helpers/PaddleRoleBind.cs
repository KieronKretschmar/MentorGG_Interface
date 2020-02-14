using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Helpers
{
    /// <summary>
    /// Paddle Role Bind
    /// Bind a PaddlePlanId to a Application Role
    /// </summary>
    public struct PaddleRoleBind
    {
        public readonly int PlanId;

        public readonly string RoleName;

        /// <summary>
        /// Create a RoleBind
        /// </summary>
        public PaddleRoleBind(int planId, string roleName)
        {
            PlanId = planId;
            RoleName = roleName;
        }
    }
}
