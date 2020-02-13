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
    /// DemoViewer controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/")]
    public class DemoViewerController : ForwardController
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
        public DemoViewerController(
            IHttpClientFactory clientFactory,
            UserManager<ApplicationUser> userManager)
        {
            _clientFactory = clientFactory;
            _userMananger = userManager;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet("match/{matchId}")]
        public async Task<IActionResult> MatchAsync(long matchId)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.MatchRetriever);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"/v1/public/match/{matchId}");

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet("match/{matchId}/round/{round}")]
        public async Task<IActionResult> RoundAsync(long matchId, int round, DemoViewerQuality quality = DemoViewerQuality.Low)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.MatchRetriever);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"/v1/public/match/{matchId}/round/{round}?quality={quality}");
            //TODO: add parameters to uri, set defaults in matchretriever

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// Enum for identifying demoviewer quality settings, e.g. frames per second.
        /// Shared by copy with (at least) MatchRetriever and the webapp. Please update accordingly.
        /// </summary>
        public enum DemoViewerQuality : byte
        {
            /// <summary>
            /// Low quality.
            /// </summary>
            Low = 1,

            /// <summary>
            /// Medium quality.
            /// </summary>
            Medium = 2,

            /// <summary>
            /// High quality.
            /// </summary>
            High = 3,
        }
    }
}