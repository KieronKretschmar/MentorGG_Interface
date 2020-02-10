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
    /// https://developer.paddle.com/webhook-reference/subscription-alerts/subscription-payment-refunded
    /// </summary>
    public class SubscriptionPaymentRefunded : IAlert
    {
        public int AlertId { get; set; }

        public string Amount { get; set; }

        public string BalanceCurrency { get; set; }

        public string BalanceEarningsDecrease { get; set; }

        public string BalanceFeeRefund { get; set; }

        public string BalanceGrossRefund { get; set; }

        public string BalanceTaxRefund { get; set; }

        public string CheckoutId { get; set; }

        public string Currency { get; set; }

        public string CustomerName { get; set; }

        public string EarningsDecrease { get; set; }

        public string Email { get; set; }

        public DateTime EventTime { get; set; }

        public string FeeRefund { get; set; }

        public string GrossRefund { get; set; }

        public string InitialPayment { get; set; }

        public string Instalments { get; set; }

        public bool MarketingConsent { get; set; }

        public string OrderId { get; set; }

        public string Passthrough { get; set; }

        public string Quantity { get; set; }

        public string RefundReason { get; set; }

        public string RefundType { get; set; }

        public string Status { get; set; }

        public string SubscriptionId { get; set; }

        public string SubscriptionPaymentId { get; set; }

        public string SubscriptionPlanId { get; set; }

        public string TaxRefund { get; set; }

        public string UnitPrice { get; set; }

        public string UserId { get; set; }

    }
}
