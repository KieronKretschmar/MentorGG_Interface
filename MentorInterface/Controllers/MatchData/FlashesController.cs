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
    /// Flashes controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/")]
    public class FlashesController : ForwardController
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
        public FlashesController(
            IHttpClientFactory clientFactory,
            UserManager<ApplicationUser> userManager)
        {
            _clientFactory = clientFactory;
            _userMananger = userManager;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("single/{steamId}/flashes")]
        public async Task<IActionResult> FlashesAsync(long steamId, string matchIds, string map)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.MatchRetriever);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/single/{steamId}/flashes?matchIds={matchIds}&map={map}");

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("single/{steamId}/flashesoverview")]
        public async Task<IActionResult> FlashesOverviewAsync(long steamId, string matchIds)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.MatchRetriever);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/single/{steamId}/flashesoverview?matchIds={matchIds}");

            return await ForwardHttpRequest(client, message);
        }
    }
}