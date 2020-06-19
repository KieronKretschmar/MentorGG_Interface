using Database;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Helpers
{
    /// <summary>
    /// Provides methods to obtain data regarding a user's roles.
    /// </summary>
    public interface IRoleHelper
    {
        /// <summary>
        /// Gets the number of matches a user may analyze daily.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<int> GetDailyMatchesLimitAsync(ApplicationUser user);

        /// <summary>
        /// Gets the user's subscription. Returns the highest one if multiple are available.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<SubscriptionType> GetSubscriptionTypeAsync(ApplicationUser user);
    }

    public class RoleHelper : IRoleHelper
    {
        private const int DAILY_LIMIT_DEFAULT = 3;
        private readonly ILogger<RoleHelper> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationContext _applicationContext;
        private readonly ISteamUserOperator _steamUserOperator;

        public RoleHelper(
            ILogger<RoleHelper> logger,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ApplicationContext applicationContext,
            ISteamUserOperator steamUserOperator)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _applicationContext = applicationContext;
            _steamUserOperator = steamUserOperator;
        }

        public async Task<int> GetDailyMatchesLimitAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return _applicationContext.Roles.Where(x => roles.Contains(x.Name))
                .Max(x => (int?) x.DailyMatchesLimit) ?? DAILY_LIMIT_DEFAULT;
        }

        public async Task<SubscriptionType> GetSubscriptionTypeAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            if(roles.Contains(RoleCreator.Ultimate.Name))
            {
                return SubscriptionType.Ultimate;
            }
            if (roles.Contains(RoleCreator.Premium.Name))
            {
                return SubscriptionType.Premium;
            }

            // Check whether user is Influencer by having MENTOR.GG in their name as we currently do not store it as a role in database
            var steamUserData = await _steamUserOperator.GetUser(user.SteamId);
            if (steamUserData.SteamName.ToLowerInvariant().Contains("mentor.gg"))
            {
                _logger.LogInformation($"User [ {steamUserData.SteamId} ] has mentor.gg in his name.");
                return SubscriptionType.Influencer;
            }

            return SubscriptionType.Free;
        }
    }
}
