using Entities.Models;
using MentorInterface.Helpers;
using MentorInterface.Paddle.PaddleApi;
using MentorInterface.Paddle.PaddleApi.Responses;
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

        Task<string> CreateReferralCouponAsync();
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
                $"api/2.0/subscription/users/update");

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

        /// <summary>
        /// Create a coupon for the ProductId 
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateReferralCouponAsync()
        {
            _logger.LogInformation($"Contacting Paddle API to create Subscription Coupon");

            var client = _clientFactory.CreateClient(ConnectedServices.PaddleApi);
            
            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Post,
                "api/2.1/product/create_coupon");

            var expiryDate = DateTime.Now + TimeSpan.FromDays(7);
            CreateCouponRequest couponRequest = new CreateCouponRequest
            {
                VendorId = _vendorId,
                VendorAuthCode = _vendorAuthCode,

                NumberOfCoupons = 1,
                CouponType = "product",
                ProductIds = "587490",
                DiscountType = "percentage",
                DiscountAmount = 100,
                Currency = "USD",
                AllowedUses = 1,
                Expires = expiryDate.ToString("yyyy-MM-dd"),
                Recurring = 0,
                Group = "referral",
            };

            message.Content = new StringContent(JsonConvert.SerializeObject(couponRequest), Encoding.UTF8, "application/json");

            CreateCouponResponse couponResponse;
            try
            {
                var responseMessage = await client.SendAsync(message);
                _logger.LogInformation($"Succeeded contacting Paddle API to create Subscription Coupon: [ {JsonConvert.SerializeObject(couponRequest)} ]");
                
                var content = await responseMessage.Content.ReadAsStringAsync();
                couponResponse = JsonConvert.DeserializeObject<CreateCouponResponse>(content);

                if(!couponResponse.Success)
                {
                    _logger.LogError($"Failed to create Paddle Coupon. [ {content} ]");
                }
            }
            catch (Exception)
            {
                _logger.LogInformation($"Failed creating Subscription Coupon from Request: [ {JsonConvert.SerializeObject(couponRequest)} ]");
                throw;
            }

            return couponResponse.FirstCoupon();
        }
    }
}
