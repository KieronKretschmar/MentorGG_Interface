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
using Swashbuckle.AspNetCore.Annotations;

namespace MentorInterface.Controllers.MatchData
{
    /// <summary>
    /// FriendComparison controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/")]
    public class FriendsComparisonController : ForwardController
    {
        /// <summary>
        /// Http Client Factory
        /// </summary>
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// User Manager
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Create the controller and inject the HTTPClient factory.
        /// </summary>
        public FriendsComparisonController(
            IHttpClientFactory clientFactory,
            UserManager<ApplicationUser> userManager)
        {
            _clientFactory = clientFactory;
            _userManager = userManager;
        }

        /// <summary>
        /// Get FriendsComparison
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("single/{steamId}/friendscomparison")]
        public async Task<IActionResult> FriendsComparisonAsync(long steamId, string matchIds, int count = 3, int offset = 0)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.MatchRetriever);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/single/{steamId}/friendscomparison?matchIds={matchIds}&count={count}&offset={offset}");

            return await ForwardHttpRequest(client, message);
        }
    }
}