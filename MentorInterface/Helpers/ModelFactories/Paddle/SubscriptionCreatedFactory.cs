using Entities.Models.Paddle.Alerts;
using System;
using System.Collections.Generic;

namespace MentorInterface.Helpers.ModelFactories.Paddle
{
    public static class SubscriptionCreatedFactory
    {
        public static SubscriptionCreated FromAlert(Dictionary<string, string> values)
        {
            try
            {
                return new SubscriptionCreated
                {
                    AlertId =               int.Parse(values["alert_id"]),
                    CancelUrl =             values["cancel_url"],
                    CheckoutId =            values["checkout_id"],
                    Currency =              values["currency"],
                    Email =                 values["email"],
                    EventTime =             DateTime.Parse(values["event_time"]),
                    MarketingConsent =      AlertParser.ParseBool(values["marketing_consent"]),
                    NextBillDate =          DateTime.Parse(values["next_bill_date"]),
                    Passthrough =           values["passthrough"],
                    Quantity =              values["quantity"],
                    Source =                values["source"],
                    Status =                values["status"],
                    SubscriptionId =        values["subscription_id"],
                    SubscriptionPlanId =    int.Parse(values["subscription_plan_id"]),
                    UnitPrice =             values["unit_price"],
                    UserId =                values["user_id"],
                    UpdateUrl =             values["update_url"]
                };
            }
            catch (Exception ex)
            {
                throw new AlertParseException($"Failed to create SubscriptionCreated", ex);
            }
        }
    }
}
