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

namespace MentorInterface.Controllers.MatchSelection
{
    /// <summary>
    /// MatchSelection controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/")]
    public class MatchSelectionController : ForwardController
    {
        /// <summary>
        /// Http Client Factory
        /// </summary>
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// User Manager
        /// </summary>
        private readonly UserManager<ApplicationUser> _userMananger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRoleHelper _roleHelper;

        /// <summary>
        /// Create the controller and inject the HTTPClient factory.
        /// </summary>
        public MatchSelectionController(
            IHttpClientFactory clientFactory,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IRoleHelper roleHelper)
        {
            _clientFactory = clientFactory;
            _userMananger = userManager;
            _roleManager = roleManager;
            _roleHelper = roleHelper;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("single/{steamId}/matchselection")]
        public async Task<IActionResult> MatchSelectionAsync(long steamId)
        {
            var user = await _userMananger.GetUserAsync(User);

            var dailyMatchesLimit = await _roleHelper.GetDailyMatchesLimitAsync(user);

            var client = _clientFactory.CreateClient(ConnectedServices.MatchRetriever);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/single/{steamId}/matchselection?dailyLimit={dailyMatchesLimit}");

            return await ForwardHttpRequest(client, message);
        }
    }
}