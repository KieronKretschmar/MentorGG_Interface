using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Payment
{
    /// <summary>
    /// Collection of Paddle Webhook alerts.
    /// https://developer.paddle.com/webhook-reference/subscription-alerts/
    /// </summary>
    public class AlertType
    {
        public const string SubscriptionCreated = "subscription_created";
        public const string SubscriptionUpdated= "subscription_updated";
        public const string SubscriptionCancelled = "subscription_cancelled";

        public const string SubscriptionPaymentSucceded = "subscription_payment_succeeded";
        public const string SubscriptionPaymentFailed = "subscription_payment_failed";
        public const string SubscriptionPaymentRefunded = "subscription_payment_refunded";
    }
}
