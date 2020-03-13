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
    /// Smokes controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/")]
    public class MisplaysController : ForwardController
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
        public MisplaysController(
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
        [HttpGet("single/{steamId}/misplays/match/{matchId}")]
        public async Task<IActionResult> MisplaysAsync(long steamId, long matchId)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.MatchRetriever);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/single/{steamId}/misplays/match/{matchId}");

            return await ForwardHttpRequest(client, message);
        }
    }
}