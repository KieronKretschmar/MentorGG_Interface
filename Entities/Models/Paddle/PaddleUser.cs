using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models.Paddle.Alerts;

namespace Entities.Models.Paddle
{
    public class PaddleUser : IPaddleUser
    {
        /// <summary>
        /// Navigational Property
        /// </summary>
        public ApplicationUser User { get; set; }

        public int Id { get; set; }

        public string UserId { get; set; }

        public string Email { get; set; }

        public bool MarketingConsent { get; set; }

        public string SubscriptionId { get; set; }

        public string SubscriptionPlanId { get; set; }

        public string Status { get; set; }

        public string CancelUrl { get; set; }

        public string UpdateUrl { get; set; }

        /// <summary>
        /// Data passed through from Paddle via the WebApp
        /// </summary>
        public string Passthrough { get; set; }

        /// <summary>
        /// SteamId associated with this PaddleUser.
        /// Devrived from Passthrough
        /// </summary>
        public long SteamId { get; set; }
    }
}
