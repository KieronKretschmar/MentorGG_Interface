using Entities.Models;
using MentorInterface.Helpers;
using MentorInterface.Paddle.PaddleApi;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MentorInterface.Paddle
{
    /// <summary>
    /// Communicates with the Paddle API.
    /// </summary>
    public interface IPaddleApiCommunicator
    {
        Task UpdateSubscriptionAsync(int subscriptionId, int planId);
    }

    public class PaddleApiCommunicator : IPaddleApiCommunicator
    {
        private readonly ILogger<PaddleApiCommunicator> _logger;

        /// <summary>
        /// Http Client Factory
        /// </summary>
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Vendor ID. Found at Paddle Dashboard -> Authentication.
        /// </summary>
        private readonly int _vendorId;

        /// <summary>
        /// Vendor Authentication code. Found at Paddle Dashboard -> Authentication.
        /// </summary>
        private readonly string _vendorAuthCode;

        /// <summary>
        /// Create the controller and inject the HTTPClient factory.
        /// </summary>
        public PaddleApiCommunicator(
            ILogger<PaddleApiCommunicator> logger,
            IHttpClientFactory clientFactory,
            int vendorId,
            string vendorAuthCode)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _vendorId = vendorId;
            _vendorAuthCode = vendorAuthCode;
        }

        /// <summary>
        /// Contacts the paddle API to update the given Subscription to the given Plan.
        /// </summary>
        /// <param name="subscriptionId">Id of the subscription to be updated.</param>
        /// <param name="planId">Id of the new plan.</param>
        /// <returns></returns>
        public async Task UpdateSubscriptionAsync(int subscriptionId, int planId)
        {
            _logger.LogInformation($"Contacting Paddle API to update subscription#{subscriptionId} to plan #{planId}");


            var client = _clientFactory.CreateClient(ConnectedServices.PaddleApi);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Post,
                $"/subscription/users/update");

            UpdateSubscriptionRequest requestModel = new UpdateSubscriptionRequest
            {
                VendorId = _vendorId,
                VendorAuthCode = _vendorAuthCode,
                PlanId = planId,
                SubscriptionId = subscriptionId,

                KeepModifiers = true,
                Prorate = true,
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");
            message.Content = stringContent;

            try
            {
                var responseMessage = await client.SendAsync(message);
                _logger.LogInformation($"Succeeded contacting Paddle API to update subscription: [ {JsonConvert.SerializeObject(requestModel)} ]");
            }
            catch (Exception)
            {
                _logger.LogInformation($"Failed contacting Paddle API to update subscription: [ {JsonConvert.SerializeObject(requestModel)} ]");
                throw;
            }
        }
    }
}
