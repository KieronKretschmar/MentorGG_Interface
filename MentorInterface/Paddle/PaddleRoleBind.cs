using Entities.Models.Paddle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Paddle
{
    /// <summary>
    /// Holds a PaddlePlan and the Application Roles it grants.
    /// </summary>
    public struct PaddlePlanRoleBind
    {
        /// <summary>
        /// The PaddlePlan.
        /// </summary>
        public readonly PaddlePlan Plan;

        /// <summary>
        /// The Roles the PaddlePlan grants.
        /// </summary>
        public readonly List<string> RoleNames;

        /// <summary>
        /// Create a RoleBind.
        /// </summary>
        public PaddlePlanRoleBind(PaddlePlan plan, List<string> roleNames)
        {
            Plan = plan;
            RoleNames = roleNames;
        }
    }
}
