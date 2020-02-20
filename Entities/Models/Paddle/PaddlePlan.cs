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
<<<<<<< HEAD
        /// </summary>
        public int PlanId { get; set; }

        /// <summary>
        /// Navigational Property
=======
>>>>>>> Made PaddlePlan and Role a many to many relationship
        /// </summary>
        public virtual ICollection<PaddlePlanRole> PaddlePlanRoles { get; set; }

        /// <summary>
        /// Navigational Property
        /// </summary>
<<<<<<< HEAD
        public virtual ICollection<PaddleSubscription> Subscriptions { get; set; }
=======
        public virtual DbSet<PaddlePlanRole> PaddlePlanRoles { get; set; }
>>>>>>> Made PaddlePlan and Role a many to many relationship
    }
}
