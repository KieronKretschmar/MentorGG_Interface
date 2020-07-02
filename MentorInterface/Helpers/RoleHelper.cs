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
        /// Gets the number of matches a list of users may analyze daily.
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        Task<List<Tuple<ApplicationUser, int>>> GetDailyMatchesLimitAsync(List<ApplicationUser> users);

        /// <summary>
        /// Gets the user's subscription. Returns the highest one if multiple are available.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<SubscriptionType> GetSubscriptionTypeAsync(ApplicationUser user);

        /// <summary>
        /// Gets the user's subscription. Returns the highest one if multiple are available. 
        /// 
        /// If <paramref name="requestedSteamId"/> is one of the demo profiles, returns ULTIMATE instead.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="requestedSteamId"></param>
        /// <returns></returns>
        Task<SubscriptionType> GetSubscriptionTypeAsync(ApplicationUser user, long requestedSteamId);

        /// <summary>
        /// Return a SubscriptionType for a list of ApplicationUsers
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        Task<List<Tuple<ApplicationUser, SubscriptionType>>> GetSubscriptionTypesAsync(List<ApplicationUser> users);
    }

    public class RoleHelper : IRoleHelper
    {
        private const int DAILY_LIMIT_DEFAULT = 3;

        /// <summary>
        /// List of steamids for which any user may access data as if they were ULTIMATE users.
        /// </summary>
        private readonly List<long> DemoProfiles = new List<long>
        {
            76561198033880857
        };

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

        public async Task<List<Tuple<ApplicationUser, int>>> GetDailyMatchesLimitAsync(List<ApplicationUser> users)
        {
            var limits = new List<Tuple<ApplicationUser, int>>(users.Count);
            foreach(var user in users)
            {
                var limit = await GetDailyMatchesLimitAsync(user);
                limits.Add(new Tuple<ApplicationUser, int>(user, limit));
            }
            return limits;
        }

        public async Task<SubscriptionType> GetSubscriptionTypeAsync(ApplicationUser user)
        {
            if(user == null)
            {
                return SubscriptionType.Free;
            }

            var roles = await _userManager.GetRolesAsync(user);

            if( roles.Contains(RoleCreator.Ultimate.Name))
            {
                return SubscriptionType.Ultimate;
            }
            if (roles.Contains(RoleCreator.Premium.Name))
            {
                return SubscriptionType.Premium;
            }

            var steamUserData = await _steamUserOperator.GetUser(user.SteamId);
            if (IsInfluencer(steamUserData.SteamName))
            {
                _logger.LogInformation($"User [ {user.SteamId} ] has mentor.gg in their name - Returning SubscriptioType.Influencer");
                return SubscriptionType.Influencer;
            }

            return SubscriptionType.Free;
        }

        public async Task<SubscriptionType> GetSubscriptionTypeAsync(ApplicationUser user, long requestedSteamId)
        {
            if (DemoProfiles.Contains(requestedSteamId))
            {
                return SubscriptionType.Ultimate;
            }

            return await GetSubscriptionTypeAsync(user);
        }

        public async Task<List<Tuple<ApplicationUser, SubscriptionType>>> GetSubscriptionTypesAsync(List<ApplicationUser> users)
        {
            var subscriptionTypes = new List<Tuple<ApplicationUser, SubscriptionType>>(users.Count);
            
            // Determine of which users are considered Influencers
            var steamUsers = await _steamUserOperator.GetUsers(users.Select(x=>x.SteamId).ToList());
            var influencers = steamUsers.Where(x => IsInfluencer(x.SteamName)).ToList();

            //Iterate over each Application user and assign a SubscriptionType
            //Based on Role information and Influencer info
            foreach(var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains(RoleCreator.Ultimate.Name))
                {
                    subscriptionTypes.Add(new Tuple<ApplicationUser, SubscriptionType>(user, SubscriptionType.Ultimate));
                    continue;
                }
                if (roles.Contains(RoleCreator.Premium.Name))
                {
                    subscriptionTypes.Add(new Tuple<ApplicationUser, SubscriptionType>(user, SubscriptionType.Premium));
                    continue;
                }

                if (influencers.Select(x => x.SteamId).Contains(user.SteamId))
                {
                    subscriptionTypes.Add(new Tuple<ApplicationUser, SubscriptionType>(user, SubscriptionType.Influencer));
                    continue;
                }

                subscriptionTypes.Add(new Tuple<ApplicationUser, SubscriptionType>(user, SubscriptionType.Free));
            }

            return subscriptionTypes;
        }

        // Check whether user is Influencer by having MENTOR.GG in their name as we currently do not store it as a role in database
        private bool IsInfluencer(string steamName)
        {
            return steamName.ToLowerInvariant().Contains("mentor.gg");
        }

    }
}
