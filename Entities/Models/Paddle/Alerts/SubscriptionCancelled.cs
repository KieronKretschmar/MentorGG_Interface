using Entities.Models.Paddle;
using Entities.Models.Paddle.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models.Paddle.Alerts
{
    /// <summary>
    /// Subscription Cancelled Webhook Alert.
    /// https://developer.paddle.com/webhook-reference/subscription-alerts/subscription-cancelled
    /// </summary>
    public class SubscriptionCancelled : IAlert
    {
        public int AlertId { get; set; }

        public DateTime CancellationEffectiveDate { get; set; }

        public string CheckoutId { get; set; }

        public string Currency { get; set; }

        public string Email { get; set; }

        public DateTime EventTime { get; set; }

        public bool MarketingConsent { get; set; }

        public string Passthrough { get; set; }

        public string Quantity { get; set; }

        public string Status { get; set; }

        public string SubscriptionId { get; set; }

        public string SubscriptionPlanId { get; set; }

        public string UnitPrice { get; set; }

        public string UserId { get; set; }
    }
}
