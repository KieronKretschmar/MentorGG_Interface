using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models.Paddle.Alerts;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models.Paddle
{
    public class PaddlePlan
    {
        /// <summary>
        /// PlanId provided by Paddle to identify the Subscription.
        /// </summary>
        public int PlanId { get; set; }

        /// <summary>
        /// Navigational Property
        /// </summary>
        public virtual ICollection<PaddlePlanRole> PaddlePlanRoles { get; set; }

        /// <summary>
        /// Navigational Property
        /// </summary>
        public virtual ICollection<PaddleSubscription> Subscriptions { get; set; }
    }
}
