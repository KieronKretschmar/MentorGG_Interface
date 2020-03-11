using Entities;
using Entities.Models.Paddle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Helpers
{
    /// <summary>
    /// Grants a user one or more Roles. Subscriptions may be granted through different PaddlePlans.
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// The type of Subscription for this Role.
        /// </summary>
        public SubscriptionType Type;

        /// <summary>
        /// The number of matches for each day users with this subscription may see.
        /// </summary>
        public int DailyMatchesLimit { get; set; }

        /// <summary>
        /// Roles granted by this subscription.
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        /// Return the Subscription Type's Enum name.
        /// </summary>
        public static implicit operator string(Subscription role)
        {
            return Enum.GetName(typeof(SubscriptionType), role.Type);
        }

        /// <summary>
        /// Return the Subscription Type's Enum name.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Enum.GetName(typeof(SubscriptionType), Type);
        }

        /// <summary>
        /// A data model representing a PaddlePlan.
        /// </summary>
        public struct Offer
        {
            public Offer(PaddlePlan plan)
            {
                SubscriptionType = plan.SubscriptionType;
                ProductId = plan.PlanId;
                Months = plan.Months;
                MonthlyPrice = plan.MonthlyPrice;
            }
            public SubscriptionType SubscriptionType{ get; set; }
            public int Months { get; set; }
            public double MonthlyPrice { get; set; }
            public int ProductId { get; set; }
        }
    }

    /// <summary>
    /// Collection of Application Roles
    /// </summary>
    public static class Subscriptions
    {
        /// <summary>
        /// All possible Subscriptions
        /// </summary>
        public static List<Subscription> All => new List<Subscription> { Premium, Ultimate };

        /// <summary>
        /// Premium
        /// </summary>
        public static Subscription Premium = new Subscription { 
            Type = SubscriptionType.Premium, 
            DailyMatchesLimit = 5,
            Roles = new List<string> { SubscriptionType.Premium.ToString() },
        };

        /// <summary>
        /// Ultimate
        /// </summary>
        public static Subscription Ultimate = new Subscription
        {
            Type = SubscriptionType.Ultimate,
            DailyMatchesLimit = 200,
            Roles = new List<string> { SubscriptionType.Ultimate.ToString() },
        };

        /// <summary>
        /// Returns the subscription.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Subscription GetSubscription(SubscriptionType type)
        {
            return All.Single(x => x.Type == type);
        }
    }

    /// <summary>
    /// Provides convenience methods when interacting with subscriptions.
    /// </summary>
    public static class SubscriptionHelper
    {
        /// <summary>
        /// The daily limit for users without a role granting them a higher daily limit.
        /// </summary>
        public static readonly int DefaultDailyLimit = 3;

        /// <summary>
        /// Gets the daily matches limit given an array of role names.
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static int GetDailyMatchesLimit(IList<string> roles)
        {
            var limit = DefaultDailyLimit;
            foreach (var role in roles)
            {
                var sub = Subscriptions.All.SingleOrDefault(x => x.ToString() == role);
                if (sub != null)
                {
                    limit = Math.Max(limit, sub.DailyMatchesLimit);
                }
            }
            return limit;
        }

    }
}
