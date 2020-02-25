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
    /// https://developer.paddle.com/webhook-reference/subscription-alerts/subscription-payment-succeeded
    /// </summary>
    public class SubscriptionPaymentSucceeded : IAlert
    {
        public int AlertId { get; set; }

        public string BalanceCurrency { get; set; }

        public string BalanceEarnings { get; set; }

        public string BalanceFee { get; set; }

        public string BalanceGross { get; set; }

        public string BalanceTax { get; set; }

        public string CheckoutId { get; set; }

        public string Country { get; set; }

        public string Coupon { get; set; }

        public string Currency { get; set; }

        public string CustomerName { get; set; }

        public string Earnings { get; set; }

        public string Email { get; set; }

        public DateTime EventTime { get; set; }

        public string Fee { get; set; }

        public string InitialPayment { get; set; }

        public string Instalments { get; set; }

        public bool MarketingConsent { get; set; }

        public DateTime NextBillDate { get; set; }

        public string NextPaymentAmount { get; set; }

        public string OrderId { get; set; }

        public string Passthrough { get; set; }

        public string PaymentTax { get; set; }

        public string PlanName { get; set; }

        public string Quantity { get; set; }

        public string ReceiptUrl { get; set; }

        public string SaleGross { get; set; }

        public string Status { get; set; }

        public string SubscriptionId { get; set; }

        public string SubscriptionPaymentId { get; set; }

        public string SubscriptionPlanId { get; set; }

        public string UnitPrice { get; set; }

        public string UserId { get; set; }

    }
}
