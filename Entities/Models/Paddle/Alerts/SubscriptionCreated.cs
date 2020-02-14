using Entities.Models.Paddle;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models.Paddle.Alerts
{
    /// <summary>
    /// Subscription Created Webhook Alert.
    /// https://developer.paddle.com/webhook-reference/subscription-alerts/subscription-created
    /// </summary>
    public class SubscriptionCreated : IPaddleUser, IAlert
    {
        public int AlertId { get; set; }

        public string CancelUrl { get; set; }

        public string CheckoutId { get; set; }

        public string Currency { get; set; }

        public string Email { get; set; }

        public DateTime EventTime { get; set; }

        public bool MarketingConsent { get; set; }

        public DateTime NextBillDate { get; set; }

        public string Passthrough { get; set; }

        public string Quantity { get; set; }

        public string Status { get; set; }

        public string Source { get; set; }

        public int SubscriptionId { get; set; }

        public int SubscriptionPlanId { get; set; }

        public string UnitPrice { get; set; }

        public int UserId { get; set; }

        public string UpdateUrl { get; set; }

    }
}
