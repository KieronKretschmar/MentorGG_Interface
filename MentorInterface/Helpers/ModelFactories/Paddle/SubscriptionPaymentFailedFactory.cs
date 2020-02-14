using Entities.Models.Paddle.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Helpers.ModelFactories.Paddle
{
    public class SubscriptionPaymentFailedFactory
    {
        public static SubscriptionPaymentFailed FromAlert(Dictionary<string, string> values)
        {
            try
            {
                return new SubscriptionPaymentFailed
                {
                    AlertId =                       int.Parse(values["alert_id"]),
                    Amount =                        values["amount"],
                    CancelUrl =                     values["cancel_url"],
                    CheckoutId =                    values["checkout_id"],
                    Currency =                      values["currency"],
                    Email =                         values["email"],
                    EventTime =                     DateTime.Parse(values["event_time"]),
                    MarketingConsent =              AlertParser.ParseBool(values["marketing_consent"]),
                    NextRetryDate =                 DateTime.Parse(values["next_retry_date"]),
                    Passthrough =                   values["passthrough"],
                    Quantity =                      values["quantity"],
                    Status =                        values["status"],
                    SubscriptionId =                int.Parse(values["subscription_id"]),
                    SubscriptionPlanId =            int.Parse(values["subscription_plan_id"]),
                    UnitPrice =                     values["unit_price"],
                    UpdateUrl =                     values["update_url"],
                    SubscriptionPaymentId =         values["subscription_payment_id"],
                    Installments =                  values["installments"],
                    OrderId =                       values["order_id"],
                    UserId =                        int.Parse(values["user_id"]),
                    AttemptNumber =                 values["attempt_number"],
                };
            }
            catch (Exception ex)
            {
                throw new AlertParseException($"Failed to create SubscriptionPaymentFailed", ex);
            }
        }
    }
}
