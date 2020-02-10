using Entities.Models.Paddle;
using Entities.Models.Paddle.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models.Paddle.Alerts
{
    /// <summary>
    /// Subscription Created Webhook Alert.
    /// https://developer.paddle.com/webhook-reference/subscription-alerts/subscription-payment-failed
    /// </summary>
    public class SubscriptionPaymentFailed : IPaddleUser, IAlert
    {
        public int AlertId { get; set; }

        public string Amount { get; set; }

        public string CancelUrl { get; set; }

        public string CheckoutId { get; set; }

        public string Currency { get; set; }

        public string Email { get; set; }

        public DateTime EventTime { get; set; }

        public bool MarketingConsent { get; set; }

        public DateTime NextRetryDate { get; set; }

        public string Passthrough { get; set; }

        public string Quantity { get; set; }

        public string Status { get; set; }

        public string SubscriptionId { get; set; }

        public string SubscriptionPlanId { get; set; }

        public string UnitPrice { get; set; }

        public string UpdateUrl { get; set; }

        public string SubscriptionPaymentId { get; set; }

        public string Installments { get; set; }

        public string OrderId { get; set; }

        public string UserId { get; set; }

        public string AttemptNumber { get; set; }

    }
}
