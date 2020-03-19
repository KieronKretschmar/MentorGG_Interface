using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Entities.Models;
using MentorInterface.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
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
        /// Query FaceItMatchGatherer to look for matches.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("faceit/look")]
        public async Task<IActionResult> FaceItAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _clientFactory.CreateClient(ConnectedServices.FaceitMatchGatherer);
            var subscriptionType = await _roleHelper.GetSubscriptionTypeAsync(user);
            var parameters = new Dictionary<string, string>()
                {
                    {"userSubscription", ((byte) subscriptionType).ToString() }
                };

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Post,
                QueryHelpers.AddQueryString($"/users/{user.SteamId}/look-for-matches", parameters));

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// Query SharingCodeGatherer to look for matches.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("valve/look")]
        public async Task<IActionResult> ValveAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _clientFactory.CreateClient(ConnectedServices.SharingCodeGatherer);
            var subscriptionType = await _roleHelper.GetSubscriptionTypeAsync(user);
            var parameters = new Dictionary<string, string>()
                {
                    {"userSubscription", ((byte) subscriptionType).ToString() }
                };

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Post,
                QueryHelpers.AddQueryString($"/users/{user.SteamId}/look-for-matches", parameters));

            return await ForwardHttpRequest(client, message);
        }

    }
}