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
        public List<PaddleSubscriptionModel> ActiveSubscriptions { get; set; }
    }

    /// <summary>
    /// Data model for a user's PaddleSubscription, including the required details for a user to manage it.
    /// </summary>
    public class PaddleSubscriptionModel
    {
        public int ApplicationUserId { get; set; }
        public int SubscriptionId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string CancelUrl { get; set; }
        public string UpdateUrl { get; set; }
        public DateTime? ExpirationTime { get; set; }

        public PaddleSubscriptionModel(PaddleSubscription paddleSubscription)
        {
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
        public SubscriptionType SubscriptionType { get; set; }

        /// <summary>
        /// List of all available offers for this subscription.
        /// </summary>
        public List<Subscription.Offer> Offers { get; set; }

        public double StartingFromMonthly => Offers.Select(x => x.MonthlyPrice).Min();
        
        public AvailableSubscription(SubscriptionType type, List<PaddlePlan> plans)
        {
            SubscriptionType = type;
            Offers = plans.Select(x=> new Subscription.Offer(x)).ToList();
        }
    }
}
