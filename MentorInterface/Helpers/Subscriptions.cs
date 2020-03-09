using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Helpers
{

    /// <summary>
    /// SubscriptionType
    /// </summary>
    public enum SubscriptionType
    {
        /// <summary>
        /// Free
        /// </summary>
        Free = 1,

        /// <summary>
        /// Premium
        /// </summary>
        Premium = 2,

        /// <summary>
        /// Ultimate
        /// </summary>
        Ultimate = 3,
    }

    /// <summary>
    /// An Application Role Helper
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
        public int DailyLimit { get; set; }

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
        public static Subscription Premium = new Subscription { Type = SubscriptionType.Premium, DailyLimit = 5 };

        /// <summary>
        /// Ultimate
        /// </summary>
        public static Subscription Ultimate = new Subscription { Type = SubscriptionType.Ultimate, DailyLimit = 200 };

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
        public static int GetDailyLimit(IList<string> roles)
        {
            var limit = DefaultDailyLimit;
            foreach (var role in roles)
            {
                var sub = Subscriptions.All.SingleOrDefault(x => x.ToString() == role);
                if (sub != null)
                {
                    limit = Math.Max(limit, sub.DailyLimit);
                }
            }
            return limit;
        }

    }
}
