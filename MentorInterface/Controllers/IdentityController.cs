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

        public IdentityController(
            UserManager<ApplicationUser> userManager
            )
        {
            this._userManager = userManager;
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

            // Select the highest possible Subscription Type if present.
            var appRoles = await _userManager.GetRolesAsync(user);
            var subscriptionType = appRoles
                .SelectMany(appRoleName => Subscriptions.All.Where(subscriptionName => subscriptionName == appRoleName))
                .Select(x => x.Type)
                .DefaultIfEmpty(SubscriptionType.Free)
                .Max();

            return new UserIdentity
            {
                ApplicationUserId = user.Id,
                SteamId = user.SteamId,
                SubscriptionType = subscriptionType,
                DailyMatchesLimit = await GetDailyMatchesLimitAsync(user)
            };
        }

        private async Task<int> GetDailyMatchesLimitAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return SubscriptionHelper.GetDailyMatchesLimit(roles);
        }
    }
}
