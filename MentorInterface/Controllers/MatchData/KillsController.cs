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
    public class KillsController : ForwardController
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
        public KillsController(
            IHttpClientFactory clientFactory,
            UserManager<ApplicationUser> userManager)
        {
            _clientFactory = clientFactory;
            _userManager = userManager;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet("single/{steamId}/filterablekills")]
        public async Task<IActionResult> KillsAsync(long steamId, string matchIds, string map)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.MatchRetriever);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/single/{steamId}/filterablekills?matchIds={matchIds}&map={map}");

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet("single/{steamId}/killsoverview")]
        public async Task<IActionResult> KillsOverviewAsync(long steamId, string matchIds)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.MatchRetriever);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/single/{steamId}/killsoverview?matchIds={matchIds}");

            return await ForwardHttpRequest(client, message);
        }
    }
}