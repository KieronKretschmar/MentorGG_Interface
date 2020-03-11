using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Entities.Models;
using MentorInterface.Models;
using MentorInterface.Paddle;
using MentorInterface.Helpers;
using Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MentorInterface.Controllers
{
    /// <summary>
    /// Controller for Verifying User Identities.
    /// </summary>
    [Route("identity")]
    public class IdentityController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoleHelper _roleHelper;

        public IdentityController(
            UserManager<ApplicationUser> userManager,
            IRoleHelper roleHelper
            )
        {
            _userManager = userManager;
            _roleHelper = roleHelper;
        }


        /// <summary>
        /// Return the currently logged in User's information.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<UserIdentity> GetIdentityAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            var subscriptionType = await _roleHelper.GetSubscriptionTypeAsync(user);
            var dailyMatchesLimit = await _roleHelper.GetDailyMatchesLimitAsync(user);

            return new UserIdentity
            {
                ApplicationUserId = user.Id,
                SteamId = user.SteamId,
                SubscriptionType = subscriptionType,
                DailyMatchesLimit = dailyMatchesLimit
            };
        }
    }
}
