using Entities.Models.Paddle.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Helpers.ModelFactories.Paddle
{
    public class SubscriptionPaymentSucceededFactory
    {
        public static SubscriptionPaymentSucceeded FromAlert(Dictionary<string, string> values)
        {
            try
            {
                return new SubscriptionPaymentSucceeded
                {
                    AlertId = int.Parse(values["alert_id"]),
                    BalanceCurrency = values["balance_currency"],
                    BalanceEarnings = values["balance_earnings"],
                    BalanceFee = values["balance_fee"],
                    BalanceGross = values["balance_gross"],
                    BalanceTax = values["balance_tax"],
                    CheckoutId = values["checkout_id"],
                    Country = values["country"],
                    Coupon = values["coupon"],
                    Currency = values["currency"],
                    CustomerName = values["customer_name"],
                    Earnings = values["earnings"],
                    Email = values["email"],
                    EventTime = DateTime.Parse(values["event_time"]),
                    Fee = values["fee"],
                    InitialPayment = values["initial_payment"],
                    Instalments = values["instalments"],
                    MarketingConsent = AlertParser.ParseBool(values["marketing_consent"]),
                    NextBillDate = DateTime.Parse(values["next_bill_date"]),
                    NextPaymentAmount = values["next_payment_amount"],
                    OrderId = values["order_id"],
                    Passthrough = values["passthrough"],
                    PaymentTax = values["payment_tax"],
                    PlanName = values["plan_name"],
                    Quantity = values["quantity"],
                    ReceiptUrl = values["receipt_url"],
                    SaleGross = values["sale_gross"],
                    Status = values["status"],
                    SubscriptionId = values["subscription_id"],
                    SubscriptionPaymentId = values["subscription_payment_id"],
                    SubscriptionPlanId = values["subscription_plan_id"],
                    UnitPrice = values["unit_price"],
                    UserId = values["user_id"]
                };
            }
            catch (Exception ex)
            {
                throw new AlertParseException($"Failed to create SubscriptionPaymentSucceeded", ex);
            }

        }
    }
}
