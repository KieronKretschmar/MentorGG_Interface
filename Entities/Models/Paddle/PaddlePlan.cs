using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models.Paddle.Alerts;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models.Paddle
{
    /// <summary>
    /// A PaddlePlan is a specific offer for a user to obtain a subscription, registered at Paddle.
    /// </summary>
    public class PaddlePlan
    {
        public PaddlePlan(int planId, SubscriptionType subscriptionType, int months, double monthlyPrice)
        {
            PlanId = planId;
            SubscriptionType = subscriptionType;
            Months = months;
            MonthlyPrice = monthlyPrice;
        }

        /// <summary>
        /// PlanId provided by Paddle to identify the Subscription.
        /// </summary>
        public int PlanId { get; set; }

        /// <summary>
        /// Identifies the subscription granted by this plan.
        /// </summary>
        public SubscriptionType SubscriptionType { get; set; }

        /// <summary>
        /// Subscription period in months.
        /// </summary>
        public int Months { get; set; }

        /// <summary>
        /// Price per month.
        /// </summary>
        public double MonthlyPrice { get; set; }

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
