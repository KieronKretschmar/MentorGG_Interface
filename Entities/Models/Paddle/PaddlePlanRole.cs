using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models.Paddle.Alerts;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models.Paddle
{
    /// <summary>
    /// Join table to allow many to many relationship between PaddlePlan and Role.
    /// </summary>
    public class PaddlePlanRole
    {
        /// <summary>
        /// PlanId provided by Paddle to Identity the Subscription
        /// </summary>
        public int PlanId { get; set; }

        /// <summary>
        /// RoleId associated with this Paddle Plan
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Navigational Property
        /// </summary>
        public virtual ApplicationRole Role { get; set; }

        /// <summary>
        /// Navigational Property
        /// </summary>
        public virtual PaddlePlan PaddlePlan { get; set; }
    }
}
