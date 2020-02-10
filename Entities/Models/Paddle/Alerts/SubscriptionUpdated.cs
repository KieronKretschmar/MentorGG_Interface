using Entities.Models.Paddle;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models.Paddle.Alerts
{
    /// <summary>
    /// Subscription Updated Webhook Alert.
    /// https://developer.paddle.com/webhook-reference/subscription-alerts/subscription-updated
    /// </summary>
    public class SubscriptionUpdated : IPaddleUser, IAlert
    {
        public int AlertId { get; set; }

        public string CancelUrl { get; set; }

        public string CheckoutId { get; set; }

        public string Currency { get; set; }

        public string Email { get; set; }

        public DateTime EventTime { get; set; }

        public bool MarketingConsent { get; set; }

        public string Passthrough { get; set; }

        public string UserId { get; set; }

        public string UpdateUrl { get; set; }

        public string SubscriptionId { get; set; }

        public DateTime PausedAt { get; set; }
        public DateTime PausedFrom { get; set; }


        public string NewPrice { get; set; }
        public string OldPrice { get; set; }


        public string NewQuantity { get; set; }
        public string OldQuantity { get; set; }


        public string NewUnitPrice { get; set; }
        public string OldUnitPrice { get; set; }


        public DateTime NextBillDate { get; set; }
        public DateTime OldNextBillDate { get; set; }


        public string Status { get; set; }
        public string OldStatus { get; set; }


        public string SubscriptionPlanId { get; set; }
        public string OldSubscriptionPlanId { get; set; }

    }
}
