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

namespace MentorInterface.Controllers.AutomaticUpload
{
    /// <summary>
    /// Look for matches controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/automatic-upload/")]
    public class LookForMatchesController : ForwardController
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
        public LookForMatchesController(
            IHttpClientFactory clientFactory,
            UserManager<ApplicationUser> userManager)
        {
            _clientFactory = clientFactory;
            _userManager = userManager;
        }

        /// <summary>
        /// Query FaceItMatchGatherer to look for matches.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("faceit/look")]
        public async Task<IActionResult> FaceItAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _clientFactory.CreateClient(ConnectedServices.FaceitMatchGatherer);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Post,
                $"/users/{user.SteamId}/look-for-matches");

            return await ForwardHttpRequest(client, message);
        }

        /// <summary>
        /// Query SharingCodeGatherer to look for matches.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("valve/look")]
        public async Task<IActionResult> ValveAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _clientFactory.CreateClient(ConnectedServices.SharingCodeGatherer);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Post,
                $"/users/{user.SteamId}/look-for-matches");

            return await ForwardHttpRequest(client, message);
        }

    }
}