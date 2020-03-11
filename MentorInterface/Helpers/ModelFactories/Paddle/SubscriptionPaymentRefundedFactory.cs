using Entities.Models.Paddle.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Helpers.ModelFactories.Paddle
{
    public class SubscriptionPaymentRefundedFactory
    {
        public static SubscriptionPaymentRefunded FromAlert(Dictionary<string, string> values)
        {
            try
            {
                return new SubscriptionPaymentRefunded
                {
                    AlertId = int.Parse(values["alert_id"]),
                    Amount = values["amount"],
                    BalanceCurrency = values["balance_currency"],
                    BalanceEarningsDecrease = values["balance_earnings_decrease"],
                    BalanceFeeRefund = values["balance_fee_refund"],
                    BalanceGrossRefund = values["balance_gross_refund"],
                    BalanceTaxRefund = values["balance_tax_refund"],
                    CheckoutId = values["checkout_id"],
                    Currency = values["currency"],
                    CustomerName = values["customer_name"],
                    EarningsDecrease = values["earnings_decrease"],
                    Email = values["email"],
                    EventTime = DateTime.Parse(values["event_time"]),
                    FeeRefund = values["free_refund"],
                    GrossRefund = values["gross_refund"],
                    InitialPayment = values["initial_payment"],
                    Instalments = values["installments"],
                    MarketingConsent = AlertParser.ParseBool(values["marketing_consent"]),
                    OrderId = values["order_id"],
                    Passthrough = values["passthrough"],
                    Quantity = values["quantity"],
                    RefundReason = values["refund_reason"],
                    RefundType = values["refund_type"],
                    Status = values["status"],
                    SubscriptionId = values["subscription_id"],
                    SubscriptionPaymentId = values["subscription_payment_id"],
                    SubscriptionPlanId = values["subscription_plan_id"],
                    TaxRefund = values["tax_refund"],
                    UnitPrice = values["unit_price"],
                    UserId = values["user_id"]
                };
            }
            catch (Exception ex)
            {
                throw new AlertParseException($"Failed to create SubscriptionPaymentRefunded", ex);
            }
        }
    }
}
