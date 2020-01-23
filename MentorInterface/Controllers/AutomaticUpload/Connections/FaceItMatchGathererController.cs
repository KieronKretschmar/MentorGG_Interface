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
using Microsoft.AspNetCore.WebUtilities;
using Swashbuckle.AspNetCore.Annotations;

namespace MentorInterface.Controllers.AutomaticUpload
{
    /// <summary>
    /// Communicator for the managing user connections for the FaceItMatchGatherer service.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/automatic-upload/connections/faceit")]
    public class FaceItMatchGathererController : ForwardController
    {
        /// <summary>
        /// Http Client Factory
        /// </summary>
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// User Mananger
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Create the controller and inject the HTTPClient factory.
        /// </summary>
        public FaceItMatchGathererController(
            IHttpClientFactory clientFactory,
            UserManager<ApplicationUser> userMananger)
        {
            _clientFactory = clientFactory;
            _userManager = userMananger;
        }


        /// <summary>
        /// Query FaceItMatchGatherer for all information pertaining to the current user.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Connections" })]
        public async Task<IActionResult> StatusAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _clientFactory.CreateClient(ConnectedServices.FaceitMatchGatherer);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"/api/Users/{user.SteamId}");

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// Query FaceItMatchGatherer to establish a connection with the FaceIt API for the current user.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Connections" })]
        public async Task<IActionResult> ConnectUserAsync(string code)
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _clientFactory.CreateClient(ConnectedServices.FaceitMatchGatherer);

            var parameters = new Dictionary<string, string>()
                {
                    {"code", code}
                };

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Post,
                QueryHelpers.AddQueryString($"/api/Users/{user.SteamId}", parameters));

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// Query FaceItMatchGatherer to remove the connection with the FaceIt API for the current user.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Connections" })]
        public async Task<IActionResult> DisconnectUserAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _clientFactory.CreateClient(ConnectedServices.FaceitMatchGatherer);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Delete,
                $"/api/Users/{user.SteamId}");

            return await ForwardHttpRequest(client, message);
        }

    }
}