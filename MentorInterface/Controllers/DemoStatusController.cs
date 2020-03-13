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

namespace MentorInterface.Controllers
{
    /// <summary>
    /// Connection status controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/")]
    public class DemoStatusController : ForwardController
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
        public DemoStatusController(
            IHttpClientFactory clientFactory,
            UserManager<ApplicationUser> userManager)
        {
            _clientFactory = clientFactory;
            _userManager = userManager;
        }

        /// <summary>
        /// Query DemoCentral to return the demos uploaded by the given user where analysis failed.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("single/{steamId}/demostatus/failed-demos")]
        public async Task<IActionResult> FailedDemosAsync(long steamId, int count, int offset = 0)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.DemoCentral);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"/v1/public/single/{steamId}/failedmatches?count={count}&offset={offset}");

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// Query DemoCentral to return the number of matches the user currently has in queue.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("single/{steamId}/demostatus/matches-in-queue")]
        public async Task<IActionResult> MatchesInQueueAsync(long steamId)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.DemoCentral);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"/v1/public/single/{steamId}/matchesinqueue");

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// Query DemoCentral to return the position of a demo in the queue.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("match/{matchId}/demostatus/queue-position")]
        public async Task<IActionResult> QueuePositionAsync(long matchId)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.DemoCentral);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"/v1/public/match/{matchId}/queueposition");

            return await ForwardHttpRequest(client, message);
        }
    }
}