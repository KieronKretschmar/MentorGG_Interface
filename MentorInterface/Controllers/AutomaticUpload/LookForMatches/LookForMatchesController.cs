using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using MentorInterface.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace MentorInterface.Controllers.AutomaticUpload
{
    /// <summary>
    /// Look for matches controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/automatic-upload/")]
    public class LookForMatchesController : ForwardController
    {
        /// <summary>
        /// Http Client Factory
        /// </summary>
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// User Manager
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoleHelper _roleHelper;

        /// <summary>
        /// Create the controller and inject the HTTPClient factory.
        /// </summary>
        public LookForMatchesController(
            IHttpClientFactory clientFactory,
            UserManager<ApplicationUser> userManager,
            IRoleHelper roleHelper)
        {
            _clientFactory = clientFactory;
            _userManager = userManager;
            _roleHelper = roleHelper;
        }

        /// <summary>
        /// Query FaceItMatchGatherer to look for matches of the logged-in User.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("faceit/look")]
        public async Task<IActionResult> FaceItAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _clientFactory.CreateClient(ConnectedServices.FaceitMatchGatherer);
            var subscriptionType = await _roleHelper.GetSubscriptionTypeAsync(user);
            var requestedQuality = GetQualityBySubscription(subscriptionType);
            var parameters = new Dictionary<string, string>()
                {
                    {"requestedQuality", ((byte) requestedQuality).ToString() }
                };

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Post,
                QueryHelpers.AddQueryString($"/users/{user.SteamId}/look-for-matches", parameters));

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// Query FaceItMatchGatherer to look for matches for the User with the given steamId.
        /// This is a temporary endpoint for filling the database pre-release.
        /// </summary>
        /// <returns></returns>
        [HttpPost("faceit/look/{steamId}")]
        public async Task<IActionResult> FaceItAsync(long steamId)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.FaceitMatchGatherer);
            var parameters = new Dictionary<string, string>()
                {
                    {"requestedQuality", ((byte) AnalyzerQuality.Low).ToString() }
                };

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Post,
                QueryHelpers.AddQueryString($"/users/{steamId}/look-for-matches", parameters));

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// Query SharingCodeGatherer to look for matches of the logged-in User.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("valve/look")]
        public async Task<IActionResult> ValveAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _clientFactory.CreateClient(ConnectedServices.SharingCodeGatherer);
            var subscriptionType = await _roleHelper.GetSubscriptionTypeAsync(user);
            var requestedQuality = GetQualityBySubscription(subscriptionType);
            var parameters = new Dictionary<string, string>()
                {
                    {"requestedQuality", ((byte) requestedQuality).ToString() }
                };

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Post,
                QueryHelpers.AddQueryString($"/users/{user.SteamId}/look-for-matches", parameters));

            return await ForwardHttpRequest(client, message);
        }


        /// <summary>
        /// Query SharingCodeGatherer to look for matches for the User with the given steamId.
        /// This is a temporary endpoint for filling the database pre-release.
        /// </summary>
        /// <returns></returns>
        [HttpPost("valve/look/{steamId}")]
        public async Task<IActionResult> ValveAsync(long steamId)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.SharingCodeGatherer);
            var parameters = new Dictionary<string, string>()
                {
                    {"requestedQuality", ((byte) AnalyzerQuality.Low).ToString() }
                };

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Post,
                QueryHelpers.AddQueryString($"/users/{steamId}/look-for-matches", parameters));

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// Determines the AnalyzerQuality for a given subscription.
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        private AnalyzerQuality GetQualityBySubscription(SubscriptionType subscription)
        {
            switch (subscription)
            {
                case SubscriptionType.Free:
                case SubscriptionType.Influencer:
                    return AnalyzerQuality.Low;
                case SubscriptionType.Premium:
                    return AnalyzerQuality.Medium;
                case SubscriptionType.Ultimate:
                    return AnalyzerQuality.High;
                default:
                    return AnalyzerQuality.Low;
            }
        }

        /// <summary>
        /// The quality with which a demo should be analyzed, determining e.g. the FPS with which positions are stored.
        /// Also defined in rabbit-repo. Please update accordingly.
        /// </summary>
        private enum AnalyzerQuality : byte
        {
            Low = 10,
            Medium = 20,
            High = 30,
        }
    }
}