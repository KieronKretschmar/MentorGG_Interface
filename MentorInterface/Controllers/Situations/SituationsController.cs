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
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace MentorInterface.Controllers.MatchSelection
{
    /// <summary>
    /// MatchSelection controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/")]
    public class SituationsController : ForwardController
    {
        private readonly ILogger<SituationsController> _logger;

        /// <summary>
        /// Http Client Factory
        /// </summary>
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Create the controller and inject the HTTPClient factory.
        /// </summary>
        public SituationsController(
            ILogger<SituationsController> logger,
            IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }
        
        /// <summary>
        /// Return a Match's Situations.
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("single/match/{matchId}/situations")]
        public async Task<IActionResult> Match(long matchId)
        {
            _logger.LogInformation($"Getting Match Situations for: Match [ {matchId} ]");

            var client = _clientFactory.CreateClient(ConnectedServices.SituationOperator);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/match/{matchId}/situations");

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// Return a player's Situtations.
        /// </summary>
        /// <param name="steamId">SteamId of the Player</param>
        /// <param name="matchIds">Collection of MatchIds to return Situtations for</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("single/{steamId}/situations")]
        public async Task<IActionResult> Player(long steamId, string matchIds)
        {
            _logger.LogInformation($"Getting Player Situations for: SteamId [ {steamId} ]");

            var client = _clientFactory.CreateClient(ConnectedServices.SituationOperator);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/player/{steamId}/situations?matchIds={matchIds}");

            return await ForwardHttpRequest(client, message);
        }


    }
}