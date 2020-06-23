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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoleHelper _roleHelper;

        /// <summary>
        /// Http Client Factory
        /// </summary>
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Create the controller and inject the HTTPClient factory.
        /// </summary>
        public SituationsController(
            ILogger<SituationsController> logger,
            UserManager<ApplicationUser> userManager,
            IRoleHelper roleHelper,
            IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _userManager = userManager;
            _roleHelper = roleHelper;
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
            var user = await _userManager.GetUserAsync(User);
            int subType = (int)await _roleHelper.GetSubscriptionTypeAsync(user);

            var client = _clientFactory.CreateClient(ConnectedServices.SituationOperator);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/match/{matchId}/situations?subscriptionType={subType}");

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// Return a player's Situations.
        /// </summary>
        /// <param name="steamId">SteamId of the Player</param>
        /// <param name="matchIds">Collection of MatchIds to return Situations for</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("single/{steamId}/situations")]
        public async Task<IActionResult> Player(long steamId, string matchIds)
        {
            var user = await _userManager.GetUserAsync(User);
            int subType = (int)await _roleHelper.GetSubscriptionTypeAsync(user, steamId);

            var client = _clientFactory.CreateClient(ConnectedServices.SituationOperator);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/player/{steamId}/situations?matchIds={matchIds}&subscriptionType={subType}");

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// Return a player's Situations of a specific SituationType.
        /// </summary>
        /// <param name="steamId">SteamId of the Player</param>
        /// <param name="situationType">The type of which Situations are returned.</param>
        /// <param name="matchIds">Collection of MatchIds to return Situations for</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("single/{steamId}/situations/{situationType}")]
        public async Task<IActionResult> PlayerSituations(long steamId, int situationType, string matchIds)
        {
            var user = await _userManager.GetUserAsync(User);
            int subType = (int)await _roleHelper.GetSubscriptionTypeAsync(user, steamId);

            var client = _clientFactory.CreateClient(ConnectedServices.SituationOperator);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/player/{steamId}/situations/{situationType}?matchIds={matchIds}&subscriptionType={subType}");

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// Get the most recent situations of the specified type and conditions.
        /// 
        /// Used for debugging.
        /// </summary>
        /// <param name="situationType">The type of which Situations are returned.</param>
        /// <param name="matchCount">Number of most recently added matches for which to return Situations for.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("situations/situationType/{situationType}/samples-by-matchcount")]
        public async Task<IActionResult> SituationSamplesAsync(int situationType, int matchCount)
        {
            var user = await _userManager.GetUserAsync(User);
            int subType = (int)await _roleHelper.GetSubscriptionTypeAsync(user);

            var client = _clientFactory.CreateClient(ConnectedServices.SituationOperator);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/situationType/{situationType}/samples-by-matchcount?matchCount={matchCount}&subscriptionType={subType}");

            return await ForwardHttpRequest(client, message);
        }


        /// <summary>
        /// Get the most recent situations of the specified type and conditions.
        /// 
        /// Used for debugging.
        /// </summary>
        /// <param name="situationType">The type of which Situations are returned.</param>
        /// <param name="matchIds">Matches for which to return situations.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("situations/situationType/{situationType}/samples")]
        public async Task<IActionResult> SituationSamplesAsync(int situationType, string matchIds)
        {
            var user = await _userManager.GetUserAsync(User);
            int subType = (int)await _roleHelper.GetSubscriptionTypeAsync(user);

            var client = _clientFactory.CreateClient(ConnectedServices.SituationOperator);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/situationType/{situationType}/samples?matchIds={matchIds}&subscriptionType={subType}");

            return await ForwardHttpRequest(client, message);
        }
    }
}