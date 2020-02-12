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
            // Optional Fields ( Sometimes Paddle does not send these )
            string pausedAtRaw;
            DateTime pausedAt = DateTime.MinValue;
            if (values.TryGetValue("paused_at", out pausedAtRaw))
                pausedAt = DateTime.Parse(pausedAtRaw);

            string pausedFromRaw;
            DateTime pausedFrom = DateTime.MinValue;
            if (values.TryGetValue("paused_from", out pausedFromRaw))
                pausedAt = DateTime.Parse(pausedFromRaw);

            try
            {
                return new SubscriptionUpdated
                {
                    AlertId = int.Parse(values["alert_id"]),
                    CancelUrl = values["cancel_url"],
                    CheckoutId = values["checkout_id"],
                    Currency = values["currency"],
                    Email = values["email"],
                    EventTime = DateTime.Parse(values["event_time"]),
                    MarketingConsent = AlertParser.ParseBool(values["marketing_consent"]),
                    Passthrough = values["passthrough"],
                    UserId = values["user_id"],
                    UpdateUrl = values["update_url"],
                    PausedAt = pausedAt,
                    PausedFrom = pausedFrom,
                    NewPrice = values["new_price"],
                    OldPrice = values["old_price"],
                    NewQuantity = values["new_quantity"],
                    OldQuantity = values["old_quantity"],
                    NewUnitPrice = values["new_unit_price"],
                    OldUnitPrice = values["old_unit_price"],
                    NextBillDate = DateTime.Parse(values["next_bill_date"]),
                    OldNextBillDate = DateTime.Parse(values["old_next_bill_date"]),
                    Status = values["status"],
                    OldStatus = values["old_status"],
                    SubscriptionId = values["subscription_id"],
                    SubscriptionPlanId = int.Parse(values["subscription_plan_id"]),
                    OldSubscriptionPlanId = int.Parse(values["old_subscription_plan_id"])
                };
            }
            catch (Exception ex)
            {
                throw new AlertParseException($"Failed to create SubscriptionUpdated", ex);
            }

        }
    }
}
