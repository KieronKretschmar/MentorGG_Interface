using Entities.Models.Paddle.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Helpers.ModelFactories.Paddle
{
    public static class SubscriptionUpdatedFactory
    {
        public static SubscriptionUpdated FromAlert(Dictionary<string, string> values)
        {
            return new SubscriptionUpdated
            {
                AlertId =                   values["alert_id"],
                CancelUrl =                 values["cancel_url"],
                CheckoutId =                values["checkout_id"],
                Currency =                  values["currency"],
                Email =                     values["email"],
                EventTime =                 DateTime.Parse(values["event_time"]),
                MarketingConsent =          AlertParser.ParseBool(values["marketing_consent"]),
                Passthrough =               values["passthrough"],
                Source =                    values["source"],
                UserId =                    values["user_id"],
                UpdateUrl =                 values["update_url"],
                PausedAt =                  values["paused_at"],
                PausedFrom =                values["paused_from"],
                NewPrice =                  values["new_price"],
                OldPrice =                  values["old_price"],
                NewQuantity =               values["new_quanity"],
                OldQuantity =               values["old_quantity"],
                NewUnitPrice =              values["new_unit_price"],
                OldUnitPrice =              values["old_unit_price"],
                NextBillDate =              DateTime.Parse(values["next_bill_date"]),
                OldNextBillDate =           DateTime.Parse(values["old_next_bill_date"]),
                Status =                    values["status"],
                OldStatus =                 values["old_status"],
                SubscriptionId =            values["subscription_id"],
                OldSubscriptionId =         values["old_subscription_id"],
                SubscriptionPlanId =        values["subscription_plan_id"],
                OldSubscriptionPlanId =     values["old_subscription_plan_id"]

            };
        }
    }
}
