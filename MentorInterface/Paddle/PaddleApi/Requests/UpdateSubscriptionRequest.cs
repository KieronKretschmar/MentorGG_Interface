using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Paddle.PaddleApi
{
    /// <summary>
    /// Data sent to paddle API to update a subscription.
    /// </summary>
    public class UpdateSubscriptionRequest : PaddleApiRequest
    {
        /// <summary>
        /// The ID of the user's current subscription should be changed.
        /// </summary>
        [JsonProperty(PropertyName = "subscription_id")]
        public int SubscriptionId { get; set; }

        /// <summary>
        /// The ID of the new Plan that the user wants to subscribe to.
        /// </summary>
        [JsonProperty(PropertyName = "plan_id")]
        public int PlanId { get; set; }

        /// <summary>
        /// The ID of the new Plan that the user wants to subscribe to.
        /// </summary>
        [JsonProperty(PropertyName = "keep_modifiers")]
        public bool KeepModifiers { get; set; }

        /// <summary>
        /// The ID of the new Plan that the user wants to subscribe to.
        /// </summary>
        [JsonProperty(PropertyName = "prorate")]
        public bool Prorate { get; set; }
    }
}
