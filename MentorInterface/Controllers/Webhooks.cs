using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Entities.Models;
using Entities.Models.Paddle;
using Entities.Models.Paddle.Alerts;
using MentorInterface.Helpers.ModelFactories;
using MentorInterface.Helpers.ModelFactories.Paddle;
using MentorInterface.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MentorInterface.Controllers
{
    /// <summary>
    /// Controller to receive Paddle (Payment Provider) Hooks
    /// </summary>
    [Route("webhooks")]
    public class Webhooks : ControllerBase
    {

        readonly WebhookVerifier _webhookVerifier;
        private readonly ILogger<Webhooks> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PaddleUserMananger _paddleUserMananger;
        private readonly ApplicationContext _applicationContext;

        /// <summary>
        ///
        /// </summary>
        public Webhooks(
            ILogger<Webhooks> logger,
            UserManager<ApplicationUser> userManager,
            PaddleUserMananger paddleUserMananger,
            ApplicationContext applicationContext)
        {

            _webhookVerifier = new WebhookVerifier("x");
            _logger = logger;
            _userManager = userManager;
            _paddleUserMananger = paddleUserMananger;
            _applicationContext = applicationContext;
        }

        /// <summary>
        /// Paddle Webhook Receiver.
        /// </summary>
        /// <param name="rawAlert">Form Content</param>
        /// <returns></returns>
        [HttpPost("paddle")]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult HandleAlert([FromForm]Dictionary<string, string> rawAlert)
        {
            string alertName;
            try
            {
                alertName = rawAlert["alert_name"];
            }
            catch (KeyNotFoundException)
            {
                return StatusCode(400);
            }

            // Confirm the alert is valid
            if (!_webhookVerifier.IsAlertValid(rawAlert))
            {
                // Signature mismatch
                return StatusCode(401);
            }

            try
            {
                switch (alertName)
                {
                    case AlertType.SubscriptionCreated:
                        var createdAlert = SubscriptionCreatedFactory.FromAlert(rawAlert);
                        return CreateSubscription(createdAlert);

                    case AlertType.SubscriptionUpdated:
                        var updatedAlert = SubscriptionUpdatedFactory.FromAlert(rawAlert);
                        return UpdateSubscription(updatedAlert);

                    case AlertType.SubscriptionCancelled:
                        var cancelledAlert = SubscriptionCancelledFactory.FromAlert(rawAlert);
                        return CancelSubscription(cancelledAlert);

                    case AlertType.SubscriptionPaymentSucceded:
                        var paymentSucceededAlert = SubscriptionPaymentSucceededFactory.FromAlert(rawAlert);
                        _applicationContext.SubscriptionPaymentSucceeded.Add(paymentSucceededAlert);
                        _applicationContext.SaveChanges();
                        return StatusCode(200);

                    case AlertType.SubscriptionPaymentFailed:
                        var paymentFailedAlert = SubscriptionPaymentFailedFactory.FromAlert(rawAlert);
                        _applicationContext.SubscriptionPaymentFailed.Add(paymentFailedAlert);
                        _applicationContext.SaveChanges();
                        return StatusCode(200);

                    case AlertType.SubscriptionPaymentRefunded:
                        var paymentRefundedAlert = SubscriptionPaymentRefundedFactory.FromAlert(rawAlert);
                        _applicationContext.SubscriptionPaymentRefunded.Add(paymentRefundedAlert);
                        _applicationContext.SaveChanges();
                        return StatusCode(200);

                    default:
                        return StatusCode(501);
                }
            }
            catch (AlertParseException ex)
            {
                _logger.LogError(ex, "Failed to parse alert from Paddle");
                return StatusCode(400);
            }

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        private IActionResult CreateSubscription(SubscriptionCreated alert)
        {
            _applicationContext.SubscriptionCreated.Add(alert);

            PaddleUser existingUser = _applicationContext.PaddleUser.SingleOrDefault(
                x => x.UserId == alert.UserId);

            if (existingUser != null)
            {
                _logger.LogError(
                    $"Tried to create subscription for pre-existing user: [ {existingUser.UserId} ]");
                return StatusCode(400);
            }

            var newPaddleUser = PaddleUserFactory.FromAlert(alert);

            _applicationContext.PaddleUser.Add(newPaddleUser);

            // Saving the changes must happen here to get the associated ApplicationUser from
            // the Database..  ╭∩╮(Ο_Ο)╭∩╮
            _applicationContext.SaveChanges();

            // Find the assoicated ApplicationRole for this PaddlePlan
            var role = _applicationContext.PaddlePlan
                .Where(x => x.PlanId == newPaddleUser.SubscriptionPlanId)
                .Select(x => x.Role)
                .Single();

            // Find the assoicated ApplicationUser for this PaddleUser
            var user = _applicationContext.PaddleUser
                .Where(x => x.Id == newPaddleUser.Id)
                .Select(x => x.User)
                .Single();

            _userManager.AddToRoleAsync(user, role.Name).Wait();

            return StatusCode(200);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        private IActionResult UpdateSubscription(SubscriptionUpdated alert)
        {
            _applicationContext.SubscriptionUpdated.Add(alert);

            PaddleUser userInfo = PaddleUserFactory.FromAlert(alert);
            _applicationContext.UpdatePaddleUser(userInfo);
            return StatusCode(200);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        private IActionResult CancelSubscription(SubscriptionCancelled alert)
        {
            _applicationContext.SubscriptionCancelled.Add(alert);

            PaddleUser userInfo = PaddleUserFactory.FromCancelledAlert(alert);
            _applicationContext.UpdatePaddleUser(userInfo);
            return StatusCode(200);
        }
    }
}