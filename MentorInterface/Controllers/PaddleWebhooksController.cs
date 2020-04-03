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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MentorInterface.Controllers
{
    /// <summary>
    /// Controller to receive Paddle (Payment Provider) Hooks
    /// </summary>
    [Route("webhooks")]
    public class PaddleWebhooksController : ControllerBase
    {

        readonly IWebhookVerifier _webhookVerifier;
        private readonly ILogger<PaddleWebhooksController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationContext _applicationContext;

        /// <summary>
        ///
        /// </summary>
        public PaddleWebhooksController(
            ILogger<PaddleWebhooksController> logger,
            UserManager<ApplicationUser> userManager,
            IWebhookVerifier webhookVerifier,
            ApplicationContext applicationContext)
        {
            _logger = logger;
            _userManager = userManager;
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
                _logger.LogCritical("Signature Mismatch, Either someone is sending fake Paddle alerts or the PaddlePublicKey is invalid!");
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

                        // Act
                        await CreateSubscriptionAsync(createdAlert);

                        // Store the alert in the database
                        await _applicationContext.SubscriptionCreated.AddAsync(createdAlert);
                        await _applicationContext.SaveChangesAsync();

                        return StatusCode(200);
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

                        // Act
                        await UpdateSubscriptionAsync(updatedAlert);

                        // Store the alert in the database
                        await _applicationContext.SubscriptionUpdated.AddAsync(updatedAlert);
                        await _applicationContext.SaveChangesAsync();

                        return StatusCode(200);
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

                        // Act
                        await CancelSubscriptionAsync(cancelledAlert);

                        // Store the alert in the database
                        await _applicationContext.SubscriptionCancelled.AddAsync(cancelledAlert);
                        await _applicationContext.SaveChangesAsync();

                        return StatusCode(200);
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
        /// Add Role to ApplicationUser as defined by passthrough.
        /// 
        /// 
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        private async Task CreateSubscriptionAsync(SubscriptionCreated alert)
        {
            // Identify ApplicationUser
            var appUser = await GetApplicationUserFromPassthroughAsync(alert.Passthrough);

            // Make sure the user has no other subscription active
            var otherActiveSubscriptions = _applicationContext.PaddleSubscription
                .Where(x => x.ApplicationUserId == appUser.Id && !(x.ExpirationTime < DateTime.Now))
                .ToList();
            if (otherActiveSubscriptions.Any())
            {
                var errorMsg = $"ApplicationUser [ {appUser.Id} ] tried to create a new subscription, but already has subscriptions with " +
                    $"ID's [ {otherActiveSubscriptions.Select(x=>x.SubscriptionId.ToString()).ToArray()} ] in the database. SubscriptionCancelledAlert: [ {alert} ].";
                _logger.LogError(errorMsg);
                throw new Exception(errorMsg);
            }

            // Create PaddleSubscription and write to database
            var subscription = new PaddleSubscription
            {
                ApplicationUserId = appUser.Id,
                CancelUrl = alert.CancelUrl,
                SubscriptionId = alert.SubscriptionId,
                SubscriptionPlanId = alert.SubscriptionPlanId,
                UpdateUrl = alert.UpdateUrl,
                ExpirationTime = null,
            };
            _applicationContext.PaddleSubscription.Add(subscription);
            await _applicationContext.SaveChangesAsync();

            // Add role(s) to ApplicationUser
            var roles = _applicationContext.PaddlePlanRole.Where(x => x.PlanId == subscription.SubscriptionPlanId).Select(x => x.Role.Name);
            await _userManager.AddToRolesAsync(subscription.User, roles);
        }

        /// <summary>
        /// Webhook for SubscriptionUpdated.
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        private async Task UpdateSubscriptionAsync(SubscriptionUpdated alert)
        {
            // Identify applicationUser
            var appUser = await GetApplicationUserFromPassthroughAsync(alert.Passthrough);
            if (appUser == null)
            {
                var errorMsg = $"ApplicationUser [ {appUser.Id} ] updated, but was not found in the database. SubscriptionCancelledAlert: [ {alert} ].";
                _logger.LogError(errorMsg);
                throw new Exception(errorMsg);
            }

            // Remove role(s) from old plan
            var oldRoles = _applicationContext.PaddlePlanRole.Where(x => x.PlanId == alert.SubscriptionPlanId).Select(x => x.Role.Name);
            await _userManager.RemoveFromRolesAsync(appUser, oldRoles);

            // Update Subscription to new plan
            var subscription = _applicationContext.PaddleSubscription.Single(x => x.SubscriptionId == alert.SubscriptionId);
            subscription.SubscriptionPlanId = alert.SubscriptionPlanId;
            subscription.CancelUrl = alert.CancelUrl;
            subscription.UpdateUrl = alert.UpdateUrl;
            // ... reset ExpirationTime
            subscription.ExpirationTime = null;
            await _applicationContext.SaveChangesAsync();

            // Add role(s) from new plan
            var newRoles = _applicationContext.PaddlePlanRole.Where(x => x.PlanId == alert.SubscriptionPlanId).Select(x => x.Role.Name);
            await _userManager.AddToRolesAsync(subscription.User, newRoles);

            var logMsg = $"ApplicationUser [ {appUser.Id} ] updated to SubscriptionPlan#{subscription.SubscriptionPlanId}. SubscriptionUpdatedAlert: [ {alert} ].";
            _logger.LogInformation(logMsg);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        private async Task CancelSubscriptionAsync(SubscriptionCancelled alert)
        {
            //1. Identify applicationUser
            var appUser = await GetApplicationUserFromPassthroughAsync(alert.Passthrough);
            if(appUser == null)
            {
                var errorMsg = $"ApplicationUser [ {appUser.Id} ] cancelled, but was not found in the database. SubscriptionCancelledAlert: [ {alert} ].";
                _logger.LogError(errorMsg);
                throw new Exception(errorMsg);
            }

            //2. Determine PaddleSubscription from DB
            var paddleSubscription = _applicationContext.PaddleSubscription.Find(alert.SubscriptionId);
            if (paddleSubscription == null)
            {
                var errorMsg = $"PaddleSubscription [ {paddleSubscription.SubscriptionId} ] cancelled, but was not found in the database. SubscriptionCancelledAlert: [ {alert} ].";
                _logger.LogError(errorMsg);
                throw new Exception(errorMsg);
            }

            //3. Mark PaddleSubscription for deletion
            paddleSubscription.ExpirationTime = alert.CancellationEffectiveDate;
            await _applicationContext.SaveChangesAsync();
            

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

        /// <summary>
        /// Gets application user by the ApplicationUserId from inside the passthrough. 
        /// Returns null if not found.
        /// </summary>
        /// <param name="passthrough"></param>
        /// <returns></returns>
        private async Task<ApplicationUser> GetApplicationUserFromPassthroughAsync(string passthrough)
        {
            JObject jsonObj = JObject.Parse(passthrough);
            int appUserId = jsonObj.Value<int>("ApplicationUserId");
            var appUser = await _applicationContext.Users.FindAsync(appUserId);
            return appUser;
        }
    }
}