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
        private readonly UserManager<ApplicationUser> _userMananger;

        /// <summary>
        /// Create the controller and inject the HTTPClient factory.
        /// </summary>
        public MatchesController(
            IHttpClientFactory clientFactory,
            UserManager<ApplicationUser> userManager)
        {
            _clientFactory = clientFactory;
            _userMananger = userManager;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ValidateMatchIds]
        [Authorize]
        [HttpGet("single/{steamId}/matches")]
        public async Task<IActionResult> MatchesAsync(long steamId, string matchIds, int offset = 0)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.MatchRetriever);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/single/{steamId}/matches?matchIds={matchIds}&offset={offset}");

            return await ForwardHttpRequest(client, message);
        }
    }
}