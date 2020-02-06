using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MentorInterface.Payment;
using MentorInterface.Payment.IncomingModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MentorInterface.Controllers
{
    /// <summary>
    /// Controller to receive Paddle (Payment Provider) Hooks
    /// </summary>
    [Route("webhooks")]
    public class Webhooks : ControllerBase
    {

        WebhookVerifier _webhookVerifier;
        /// <summary>
        ///
        /// </summary>
        public Webhooks()
        {
            _webhookVerifier = new WebhookVerifier("x");
        }

        /// <summary>
        /// Paddle Webhook Receiver.
        /// </summary>
        /// <param name="formContent">Form Content</param>
        /// <returns></returns>
        [HttpPost("paddle")]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult HandleAlert([FromForm]Dictionary<string, string> formContent)
        {
            string alertName;
            try
            {
                alertName = formContent["alert_name"];
            }
            catch (KeyNotFoundException)
            {
                return StatusCode(400);
            }

            switch (alertName)
            {
                case AlertType.SubscriptionCreated:

                    SubscriptionCreated alert;
                    try
                    {
                        alert = (SubscriptionCreated)formContent;
                    }
                    catch (KeyNotFoundException)
                    {
                        return StatusCode(400);
                    }

                    if (_webhookVerifier.IsAlertValid(alert))
                    {
                        // Upgrade user to prem etc. etc.
                    }
                    else
                    {
                        // Signature mismatch
                        return StatusCode(401);
                    }

                    return StatusCode(200);

                case AlertType.SubscriptionUpdated:
                    return StatusCode(200);

                case AlertType.SubscriptionCancelled:
                    return StatusCode(200);

                case AlertType.SubscriptionPaymentFailed:
                    return StatusCode(200);

                case AlertType.SubscriptionPaymentSucceded:
                    return StatusCode(200);

                case AlertType.SubscriptionPaymentRefunded:
                    return StatusCode(200);

                default:
                    return StatusCode(501);
            }
        }
    }
}