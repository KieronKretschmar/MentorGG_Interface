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
    /// HighExplosives controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/")]
    public class HesController : ForwardController
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
        public HesController(
            IHttpClientFactory clientFactory,
            UserManager<ApplicationUser> userManager)
        {
            _clientFactory = clientFactory;
            _userManager = userManager;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("single/{steamId}/hes")]
        public async Task<IActionResult> HesAsync(long steamId, string matchIds, string map)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.MatchRetriever);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/single/{steamId}/hes?matchIds={matchIds}&map={map}");

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("single/{steamId}/hesoverview")]
        public async Task<IActionResult> HesOverviewAsync(long steamId, string matchIds)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.MatchRetriever);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/single/{steamId}/hesoverview?matchIds={matchIds}");

            return await ForwardHttpRequest(client, message);
        }
    }
}