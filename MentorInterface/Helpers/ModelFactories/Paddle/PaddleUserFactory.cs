using Entities.Models.Paddle;
using Entities.Models.Paddle.Alerts;
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
            try
            {
                return new PaddleUser
                {
                    UserId = values["user_id"],
                    Email = values["email"],
                    MarketingConsent = AlertParser.ParseBool(values["marketing_consent"]),
                    Status = values["status"],
                    SubscriptionId = values["subscription_id"],
                    SubscriptionPlanId = values["subscription_plan_id"],
                    UpdateUrl = values["update_url"],
                    CancelUrl = values["cancel_url"],
                };
            }
            catch (Exception ex)
            {
                throw new AlertParseException("Failed to create PaddleUser", ex);
            }

        }

        public static PaddleUser FromAlert(IPaddleUser user)
        {
            try
            {
                return new PaddleUser
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    MarketingConsent = user.MarketingConsent,
                    Status = user.Status,
                    SubscriptionId = user.SubscriptionId,
                    SubscriptionPlanId = user.SubscriptionPlanId,
                    UpdateUrl = user.UpdateUrl,
                    CancelUrl = user.CancelUrl
                };
            }
            catch (Exception ex)
            {
                throw new AlertParseException("Failed to create PaddleUser", ex);
            }

        }

        public static PaddleUser FromCancelledAlert(SubscriptionCancelled alert)
        {
            try
            {
                return new PaddleUser
                {
                    UserId = alert.UserId,
                    Email = alert.Email,
                    MarketingConsent = alert.MarketingConsent,
                    Status = alert.Status,
                    SubscriptionId = alert.SubscriptionId,
                    SubscriptionPlanId = alert.SubscriptionPlanId,
                    UpdateUrl = null,
                    CancelUrl = null
                };
            }
            catch (Exception ex)
            {
                throw new AlertParseException("Failed to create PaddleUser", ex);
            }
        }
    }
}