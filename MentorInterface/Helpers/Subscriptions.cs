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
        /// Return the Subscription Type's Enum name.
        /// </summary>
        public static implicit operator string(Subscription role)
        {
            return Enum.GetName(typeof(SubscriptionType), role.Type);
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
        public static Subscription Premium = new Subscription { Type = SubscriptionType.Premium };

        /// <summary>
        /// Ultimate
        /// </summary>
        public static Subscription Ultimate = new Subscription { Type = SubscriptionType.Ultimate };

    }


}
