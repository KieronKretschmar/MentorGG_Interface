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
using MentorInterface.Paddle;
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
        private readonly PaddleUserManager _paddleUserMananger;
        private readonly ApplicationContext _applicationContext;

        /// <summary>
        ///
        /// </summary>
        public Webhooks(
            ILogger<Webhooks> logger,
            UserManager<ApplicationUser> userManager,
            PaddleUserManager paddleUserMananger,
            WebhookVerifier webhookVerifier,
            ApplicationContext applicationContext)
        {
            _logger = logger;
            _userManager = userManager;
            _paddleUserMananger = paddleUserMananger;
            _webhookVerifier = webhookVerifier;
            _applicationContext = applicationContext;
        }

        /// <summary>
        /// Paddle Webhook Receiver.
        /// </summary>
        /// <param name="rawAlert">Form Content</param>
        /// <returns></returns>
        [HttpPost("paddle")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> HandleAlertAsync([FromForm]Dictionary<string, string> rawAlert)
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
                return StatusCode(403, "Signature mismatch");
            }

            try
            {
                switch (alertName)
                {
                    #region Subscription Created
                    case AlertType.SubscriptionCreated:
                        var createdAlert = SubscriptionCreatedFactory.FromAlert(rawAlert);

                        // Confirm the Alert is unique
                        if (_applicationContext.SubscriptionCreated.Any(x => x.AlertId == createdAlert.AlertId))
                        {
                            _logger.LogError($"Received alert that has been stored previously: [ {createdAlert.AlertId} ] ");
                            return StatusCode(200);
                        }
                        return await CreateSubscriptionAsync(createdAlert);
                    #endregion

                    #region Subscription Updated
                    case AlertType.SubscriptionUpdated:
                        var updatedAlert = SubscriptionUpdatedFactory.FromAlert(rawAlert);

                        // Confirm the Alert is unique
                        if (_applicationContext.SubscriptionUpdated.Any(x => x.AlertId == updatedAlert.AlertId))
                        {
                            _logger.LogError($"Received alert that has been stored previously: [ {updatedAlert.AlertId} ] ");
                            return StatusCode(200);
                        }
                        return await UpdateSubscriptionAsync(updatedAlert);
                    #endregion

                    #region Subscription Cancelled
                    case AlertType.SubscriptionCancelled:
                        var cancelledAlert = SubscriptionCancelledFactory.FromAlert(rawAlert);

                        // Confirm the Alert is unique
                        if (_applicationContext.SubscriptionCancelled.Any(x => x.AlertId == cancelledAlert.AlertId))
                        {
                            _logger.LogError($"Received alert that has been stored previously: [ {cancelledAlert.AlertId} ] ");
                            return StatusCode(200);
                        }
                        return await CancelSubscriptionAsync(cancelledAlert);
                    #endregion

                    #region GROUP: Subscription Payments
                    #region Subscription Payment Succeded
                    case AlertType.SubscriptionPaymentSucceded:
                        var paymentSucceededAlert = SubscriptionPaymentSucceededFactory.FromAlert(rawAlert);

                        // Confirm the Alert is unique
                        if (_applicationContext.SubscriptionPaymentSucceeded.Any(x => x.AlertId == paymentSucceededAlert.AlertId))
                        {
                            _logger.LogError($"Received alert that has been stored previously: [ {paymentSucceededAlert.AlertId} ] ");
                            return StatusCode(200);
                        }
                        _applicationContext.SubscriptionPaymentSucceeded.Add(paymentSucceededAlert);
                        _applicationContext.SaveChanges();
                        return StatusCode(200);
                    #endregion

                    #region Subscription Payment Failed
                    case AlertType.SubscriptionPaymentFailed:
                        var paymentFailedAlert = SubscriptionPaymentFailedFactory.FromAlert(rawAlert);

                        // Confirm the Alert is unique
                        if (_applicationContext.SubscriptionPaymentFailed.Any(x => x.AlertId == paymentFailedAlert.AlertId))
                        {
                            _logger.LogError($"Received alert that has been stored previously: [ {paymentFailedAlert.AlertId} ] ");
                            return StatusCode(200);
                        }
                        _applicationContext.SubscriptionPaymentFailed.Add(paymentFailedAlert);
                        _applicationContext.SaveChanges();
                        return StatusCode(200);
                    #endregion

                    #region Subscription Payment Refunded
                    case AlertType.SubscriptionPaymentRefunded:
                        var paymentRefundedAlert = SubscriptionPaymentRefundedFactory.FromAlert(rawAlert);

                        // Confirm the Alert is unique
                        if (_applicationContext.SubscriptionPaymentRefunded.Any(x => x.AlertId == paymentRefundedAlert.AlertId))
                        {
                            _logger.LogError($"Received alert that has been stored previously: [ {paymentRefundedAlert.AlertId} ] ");
                            return StatusCode(200);
                        }
                        _applicationContext.SubscriptionPaymentRefunded.Add(paymentRefundedAlert);
                        _applicationContext.SaveChanges();
                        return StatusCode(200);
                    #endregion

                    #endregion

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
        private async Task<IActionResult> CreateSubscriptionAsync(SubscriptionCreated alert)
        {
            _applicationContext.SubscriptionCreated.Add(alert);

            PaddleUser existingPaddleUser = await _paddleUserMananger.GetUserAsync(alert.UserId);

            var userInfo = PaddleUserFactory.FromAlert(alert);

            // If user exists
            if (existingPaddleUser != null)
            {
                _logger.LogInformation(
                    $"Creating new subscription for pre-existing user [ {existingPaddleUser.UserId} ]");

                userInfo.UserId = existingPaddleUser.UserId;
                bool isSuccess = await _paddleUserMananger.UpdateUserAsync(userInfo);
                if (!isSuccess)
                {
                    throw new Exception($"Failed to update PaddleUser [ {userInfo.UserId} ] ");
                }
            }
            else
            {
                bool isSuccess = await _paddleUserMananger.AddUserAsync(userInfo);
                if (!isSuccess)
                {
                    throw new Exception($"Failed to add PaddleUser [ {userInfo.UserId} ] ");
                }
            }

            // Find the associated ApplicationRole for this PaddlePlan
            var role = await _applicationContext.RoleFromPaddlePlanIdAsync(
                userInfo.SubscriptionPlanId);

            // Find the associated ApplicationUser for this PaddleUser
            var user = _applicationContext.PaddleUser
                .Where(x => x.ApplicationUserId == userInfo.ApplicationUserId)
                .Select(x => x.User)
                .Single();

            await _userManager.AddToRoleAsync(user, role.Name);

            return StatusCode(200);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        private async Task<IActionResult> UpdateSubscriptionAsync(SubscriptionUpdated alert)
        {
            _applicationContext.SubscriptionUpdated.Add(alert);

            // Update the existing Paddle User with new fields.
            PaddleUser userInfo = PaddleUserFactory.FromAlert(alert);
            bool isSuccess = await _paddleUserMananger.UpdateUserAsync(userInfo);
            if (!isSuccess)
            {
                throw new Exception($"Failed to update PaddleUser [ {userInfo.UserId} ] ");
            }

            // Remove the old plans associated role
            // Add the new plans associated role
            int newPlanId = alert.SubscriptionPlanId;
            int oldPlanId = alert.OldSubscriptionPlanId;

            // Find the NEW associated ApplicationRole for this PaddlePlan
            var newRole = await _applicationContext.RoleFromPaddlePlanIdAsync(
                newPlanId);

            // Find the OLD associated ApplicationRole for this PaddlePlan
            var oldRole = await _applicationContext.RoleFromPaddlePlanIdAsync(
                oldPlanId);

            // Find the associated ApplicationUser for this PaddleUser ID
            var user = _applicationContext.PaddleUser
                .Where(x => x.ApplicationUserId == userInfo.ApplicationUserId)
                .Select(x => x.User)
                .Single();

            _userManager.RemoveFromRoleAsync(user, oldRole.Name).Wait();
            _userManager.AddToRoleAsync(user, newRole.Name).Wait();

            return StatusCode(200);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        private async Task<IActionResult> CancelSubscriptionAsync(SubscriptionCancelled alert)
        {
            _applicationContext.SubscriptionCancelled.Add(alert);

            PaddleUser userInfo = PaddleUserFactory.FromCancelledAlert(alert);

            var isSuccess = await _paddleUserMananger.UpdateUserAsync(userInfo);
            if (!isSuccess)
            {
                throw new Exception($"Failed to update PaddleUser [ {userInfo.UserId} ] ");
            }

            return StatusCode(200);

            // TODO:
            /*
            ### Poll for when Cancellation data has passed

            var x = alert.CancellationEffectiveDate;

            ### Once is has, run the following:

            var currentRole = await _applicationContext.RoleFromPaddlePlanIdAsync(
                alert.SubscriptionPlanId);

            ### Find the associated ApplicationUser for this PaddleUser ID

            var user = _applicationContext.PaddleUser
                .Where(x => x.ApplicationUserId == userInfo.ApplicationUserId)
                .Select(x => x.User)
                .Single();

            _userManager.RemoveFromRoleAsync(user, currentRole.Name);
            */
        }
    }
}