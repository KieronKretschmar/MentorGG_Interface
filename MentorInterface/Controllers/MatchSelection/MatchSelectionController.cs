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

        /// <summary>
        /// Create the controller and inject the HTTPClient factory.
        /// </summary>
        public MatchSelectionController(
            IHttpClientFactory clientFactory,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _clientFactory = clientFactory;
            _userMananger = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("single/{steamId}/matchselection")]
        public async Task<IActionResult> MatchSelectionAsync(long steamId)
        {
            var user = await _userMananger.GetUserAsync(User);
            var dailyLimit = await GetDailyLimitAsync(user);

            var client = _clientFactory.CreateClient(ConnectedServices.MatchRetriever);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"v1/public/single/{steamId}/matchselection?dailyLimit={dailyLimit}");

            return await ForwardHttpRequest(client, message);
        }

        private async Task<int> GetDailyLimitAsync(ApplicationUser user)
        {
            var roles = await _userMananger.GetRolesAsync(user);
            return 3;
            // TODO: Return depending on role
            //if(roles.Contains())
            throw new NotImplementedException();
        }
    }
}