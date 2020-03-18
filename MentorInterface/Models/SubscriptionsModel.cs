using Entities;
using Entities.Models.Paddle;
using MentorInterface.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Models
{
    /// <summary>
    /// Holds data required for the user to manage their subscriptions.
    /// </summary>
    public class SubscriptionsModel
    {
        public List<AvailableSubscription> AvailableSubscriptions { get; set; }
        public PaddleSubscriptionModel ActiveSubscription { get; set; }
    }

    /// <summary>
    /// Data model for a user's PaddleSubscription, including the required details for a user to manage it.
    /// </summary>
    public class PaddleSubscriptionModel
    {
        public SubscriptionType SubscriptionType { get; set; }
        /// <summary>
        /// ApplicationUser associated with this PaddleUser.
        /// </summary>
        public int ApplicationUserId { get; set; }
        public int SubscriptionId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string CancelUrl { get; set; }
        public string UpdateUrl { get; set; }
        public DateTime? ExpirationTime { get; set; }

        public PaddleSubscriptionModel(SubscriptionType subscriptionType, PaddleSubscription paddleSubscription)
        {
            SubscriptionType = subscriptionType;
            ApplicationUserId = paddleSubscription.ApplicationUserId;
            SubscriptionId = paddleSubscription.SubscriptionId;
            SubscriptionPlanId = paddleSubscription.SubscriptionPlanId;
            CancelUrl = paddleSubscription.CancelUrl;
            UpdateUrl = paddleSubscription.UpdateUrl;
            ExpirationTime = paddleSubscription.ExpirationTime;
        }
    }

    /// <summary>
    /// Data model for an available subscription
    /// </summary>
    public class AvailableSubscription
    {
        /// <summary>
        /// The type of Subscription
        /// </summary>
        public SubscriptionType SubscriptionType { get; set; }

        /// <summary>
        /// List of all available PaddlePlans for this subscription.
        /// </summary>
        public List<PaddlePlanModel> Plans { get; set; }

        /// <summary>
        /// The lowest monthly price for which there is a plan.
        /// </summary>
        public double StartingFromMonthly => Plans.Select(x => x.MonthlyPrice).Min();
        
        public AvailableSubscription(SubscriptionType type, List<PaddlePlan> plans)
        {
            SubscriptionType = type;
            Plans = plans.Select(x=> new PaddlePlanModel(x)).ToList();
        }



        /// <summary>
        /// A data model representing a PaddlePlan.
        /// </summary>
        public struct PaddlePlanModel
        {
            public PaddlePlanModel(PaddlePlan plan)
            {
                SubscriptionType = plan.SubscriptionType;
                ProductId = plan.PlanId;
                Months = plan.Months;
                MonthlyPrice = plan.MonthlyPrice;
            }
            public SubscriptionType SubscriptionType { get; set; }
            public int Months { get; set; }
            public double MonthlyPrice { get; set; }
            public int ProductId { get; set; }
        }
    }
}
