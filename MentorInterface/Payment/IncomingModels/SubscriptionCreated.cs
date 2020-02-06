using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Payment.IncomingModels
{
    /// <summary>
    /// Subscription Created Webhook Alert.
    /// https://developer.paddle.com/webhook-reference/subscription-alerts/subscription-created
    /// </summary>
    public class SubscriptionCreated : IPaddleAlert
    {
        public string AlertId { get; set; }

        public string AlertName { get; set; }

        public string CancelUrl { get; set; }

        public string CheckoutId { get; set; }

        public string Currency { get; set; }

        public string Email { get; set; }

        public string EventTime { get; set; }

        public string MarketingConsent { get; set; }

        public string NextBillDate { get; set; }

        public string Passthrough { get; set; }

        public string Quantity { get; set; }

        public string Source { get; set; }

        public string SubscriptionId { get; set; }

        public string SubscriptionPlanId { get; set; }

        public string UnitPrice { get; set; }

        public string UserId { get; set; }

        public string UpdateUrl { get; set; }

        public string PaddleSignature { get; set; }

        public static explicit operator SubscriptionCreated(Dictionary<string, string> values)
        {
            throw new NotImplementedException();

            return new SubscriptionCreated
            {
                AlertName = values["alert_name"],
                AlertId = values["alert_id"],
                PaddleSignature = values["p_signature"],
                Quantity = values["quantity"],
            };
        }
    }
}
