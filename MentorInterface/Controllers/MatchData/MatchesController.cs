using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Entities.Models;
using MentorInterface.Attributes;
using MentorInterface.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentorInterface.Controllers.MatchData
{
    /// <summary>
    /// Matches (Match History) controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/")]
    public class MatchesController : ForwardController
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
        public MatchesController(
            IHttpClientFactory clientFactory,
            UserManager<ApplicationUser> userManager)
        {
            _clientFactory = clientFactory;
            _userManager = userManager;
        }


        /// <summary>
        /// Returns data regarding for each match, including scoreboards.
        /// Each matchId that is neither in matchIds nor in ignoredMatchIds will be treated as being above the daily limit and thus censored.
        /// </summary>
        /// <param name="steamId"></param>
        /// <param name="matchIds">MatchIds for which uncensored data should be returned</param>
        /// <param name="count"></param>
        /// <param name="ignoredMatchIds">MatchIds for which no data should be returned.</param>
        /// <param name="offset"></param>
        [ValidateMatchIds]
        [HttpGet("single/{steamId}/matches")]
        public async Task<IActionResult> MatchesAsync(long steamId, string matchIds, int count, string ignoredMatchIds = "", int offset = 0)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.MatchRetriever);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/single/{steamId}/matches?matchIds={matchIds}&count={count}&ignoredMatchIds={ignoredMatchIds}&offset={offset}");

            return await ForwardHttpRequest(client, message);
        }
    }
}