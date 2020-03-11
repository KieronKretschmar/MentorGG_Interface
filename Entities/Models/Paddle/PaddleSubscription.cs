using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models.Paddle.Alerts;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models.Paddle
{
    public class PaddleSubscription
    {
        /// <summary>
        /// ApplicationUser associated with this PaddleUser.
        /// Devrived from Passthrough
        /// </summary>
        public int ApplicationUserId { get; set; }

        public int SubscriptionId { get; set; }

        public int SubscriptionPlanId { get; set; }

        public string CancelUrl { get; set; }

        public string UpdateUrl { get; set; }

        public DateTime? ExpirationTime { get; set; }

        /// <summary>
        /// Navigational Property
        /// </summary>
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Navigational Property
        /// </summary>
        public virtual PaddlePlan PaddlePlan { get; set; }


    }
}
