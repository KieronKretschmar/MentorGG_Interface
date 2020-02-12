using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models.Paddle.Alerts;

namespace Entities.Models.Paddle
{
    public class PaddlePlan
    {
        /// <summary>
        /// Navigational Property
        /// </summary>
        public ApplicationRole Role { get; set; }

        /// <summary>
        /// PlanId provided by Paddle to Identity the Subscription
        /// </summary>
        public int PlanId { get; set; }

        /// <summary>
        /// RoleId associated with this Paddle Plan
        /// </summary>
        public int RoleId { get; set; }

    }
}
