using Entities.Models.Paddle.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Helpers.ModelFactories.Paddle
{
    public static class SubscriptionCancelledFactory
    {
        public static SubscriptionCancelled FromAlert(Dictionary<string, string> values)
        {
            try
            {

                return new SubscriptionCancelled
                {
                    AlertId = int.Parse(values["alert_id"]),
                    CancellationEffectiveDate = DateTime.Parse(values["cancellation_effective_date"]),
                    CheckoutId = values["checkout_id"],
                    Currency = values["currency"],
                    Email = values["email"],
                    EventTime = DateTime.Parse(values["event_time"]),
                    MarketingConsent = AlertParser.ParseBool(values["marketing_consent"]),
                    Passthrough = values["passthrough"],
                    Quantity = values["quantity"],
                    Status = values["status"],
                    UserId = values["user_id"],
                    SubscriptionId = values["subscription_id"],
                    SubscriptionPlanId = values["subscription_plan_id"],
                    UnitPrice = values["unit_price"],

                };
            }
            catch (Exception ex)
            {
                throw new AlertParseException($"Failed to create SubscriptionCancelled", ex);
            }
        }
    }
}
