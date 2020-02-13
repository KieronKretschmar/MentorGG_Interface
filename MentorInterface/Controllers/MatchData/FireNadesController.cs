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
    /// FireNades controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/")]
    public class FireNadesController : ForwardController
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
        public FireNadesController(
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
        [ValidateMatchIds]
        [HttpGet("single/{steamId}/firenades")]
        public async Task<IActionResult> FireNadesAsync(long steamId, string matchIds, string map)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.MatchRetriever);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/single/{steamId}/firenades?matchIds={matchIds}&map={map}");

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// Forward request to MatchRetriever
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("single/{steamId}/firenadesoverview")]
        public async Task<IActionResult> FireNadesOverviewAsync(long steamId, string matchIds)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.MatchRetriever);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/single/{steamId}/firenadesoverview?matchIds={matchIds}");

            return await ForwardHttpRequest(client, message);
        }
    }
}