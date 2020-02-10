using Entities.Models.Paddle;
using MentorInterface.Helpers.ModelFactories;
using MentorInterface.Helpers.ModelFactories.Paddle;
using System;
using System.Collections.Generic;
using System.Text;

namespace MentorInterface.Helpers.ModelFactories.Paddle
{
    public static class PaddleUserFactory
    {
        public static PaddleUser FromAlert(Dictionary<string, string> values)
        {
            return new PaddleUser
            {
                UserId =                values["user_id"],
                Email =                 values["email"],
                MarketingConsent =      AlertParser.ParseBool(values["marketing_consent"]),
                Status =                values["status"],
                SubscriptionId =        values["subscription_id"],
                SubscriptionPlanId =    values["subscription_plan_id"],
                UpdateUrl =             values["update_url"],
                CancelUrl =             values["cancel_url"],
            };
        }
    }
}
