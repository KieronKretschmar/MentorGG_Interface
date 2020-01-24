using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Entities.Models;
using MentorInterface.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace MentorInterface.Controllers.AutomaticUpload
{
    /// <summary>
    /// Communicator for managing user connections for the SharingCodeGatherer service.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/automatic-upload/connections/valve")]
    public class SharingCodeGathererController : ForwardController
    {
        /// <summary>
        /// Logger.
        /// </summary>
        public ILogger<SharingCodeGathererController> _logger;

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
        public SharingCodeGathererController(
            IHttpClientFactory clientFactory,
            UserManager<ApplicationUser> userManager)
        {;
            _clientFactory = clientFactory;
            _userManager = userManager;
        }

        /// <summary>
        /// Query SharingCodeGatherer for all information pertaining to the current user.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Connections" })]
        public async Task<IActionResult> StatusAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _clientFactory.CreateClient(ConnectedServices.SharingCodeGatherer);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"/users/{user.SteamId}");

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// Query SharingCodeGather to create a connection with the Valve API for the current user.
        /// </summary>
        /// <returns></returns>
        ///
        [Authorize]
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Connections" })]
        public async Task<IActionResult> ConnectUserAsync(string steamAuthToken, string lastKnownSharingCode)
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _clientFactory.CreateClient(ConnectedServices.SharingCodeGatherer);

            var parameters = new Dictionary<string, string>()
                {
                    {"steamAuthToken", steamAuthToken},
                    {"lastKnownSharingCode", lastKnownSharingCode}
                };

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Post,
                QueryHelpers.AddQueryString($"/users/{user.SteamId}", parameters));

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// Query SharingCodeGather to create remove the connection with the Vavle API for the current user.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Connections" })]
        public async Task<IActionResult> DisconnectUserAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _clientFactory.CreateClient(ConnectedServices.SharingCodeGatherer);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Delete,
                $"/users/{user.SteamId}");

            return await ForwardHttpRequest(client, message);
        }

    }
}