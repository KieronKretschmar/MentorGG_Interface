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
    [Route("v{version:apiVersion}")]
    public class SituationsFeedbackController : ForwardController
    {
        private readonly ILogger<SituationsFeedbackController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Http Client Factory
        /// </summary>
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Create the controller and inject the HTTPClient factory.
        /// </summary>
        public SituationsFeedbackController(
            ILogger<SituationsFeedbackController> logger,
            UserManager<ApplicationUser> userManager,
            IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _userManager = userManager;
            _clientFactory = clientFactory;
        }
        
        /// <summary>
        /// Submits the provided situation-feedback.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("situations/feedback")]
        public async Task<IActionResult> PostFeedbackAsync(long matchId, int situationType, long situationId, bool isPositive, string comment)
        {
            var user = await _userManager.GetUserAsync(User);

            var client = _clientFactory.CreateClient(ConnectedServices.SituationOperator);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Post,
                $"v1/public/feedback/{user.SteamId}" +
                $"?matchId={matchId}" +
                $"&situationType={situationType}" +
                $"&situationId={situationId}" +
                $"&isPositive={isPositive}" +
                $"&comment={comment}");

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// Gets all the situation-feedback ever provided by the user.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("situations/feedback")]
        public async Task<IActionResult> GetFeedbackAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            var client = _clientFactory.CreateClient(ConnectedServices.SituationOperator);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/feedback/{user.SteamId}");

            return await ForwardHttpRequest(client, message);
        }


    }
}