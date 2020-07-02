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
using Database;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using MentorInterface.Attributes;

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
        private readonly ApplicationContext _applicationContext;
        private readonly ILogger<IdentityController> _logger;

        public IdentityController(
            UserManager<ApplicationUser> userManager,
            IRoleHelper roleHelper,
            ApplicationContext applicationContext,
            ILogger<IdentityController> logger
            )
        {
            _userManager = userManager;
            _roleHelper = roleHelper;
            _applicationContext = applicationContext;
            _logger = logger;
        }


        /// <summary>
        /// Return the currently logged in User's information.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<UserIdentity> GetAuthenticatedUserIdentity()
        {
            var user = await _userManager.GetUserAsync(User);
            return await GetUserIdentityAsync(user);            
        }

        /// <summary>
        /// Return UserIdentity of any known SteamId.
        /// </summary>
        [InternalHttp]
        [HttpGet("{steamId}")]
        public async Task<ActionResult<UserIdentity>> GetIdentityFromSteamIdAsync(long steamId)
        {
            var user = _applicationContext.Users.SingleOrDefault(x => x.SteamId == steamId);
            if (user != null)
            {
                return await GetUserIdentityAsync(user);
            }
            else
            {
                return NotFound($"User [ {steamId} ] not found");
            }
        }

        /// <summary>
        /// Return known UserIdentities for a list of SteamIds.
        /// </summary>
        /// <returns></returns>
        [InternalHttp]
        [HttpGet("multiple/{steamIds}")]
        public async Task<ActionResult<List<UserIdentity>>> GetIdentitiesFromSteamIdsAsync([ModelBinder(typeof(CsvModelBinder))] List<long> steamIds)
        {
            // Get a list of ApplicationUsers from the list of SteamIds
            List<ApplicationUser> users = _applicationContext.Users.Where(x=>steamIds.Contains(x.SteamId)).Select(y => y).ToList();

            if(users.Count == 0)
            {
                return NotFound($"Users not found");
            }

            var subscriptionTypes = await _roleHelper.GetSubscriptionTypesAsync(users);
            var dailyLimits = await _roleHelper.GetDailyMatchesLimitAsync(users);

            List<UserIdentity> identities = new List<UserIdentity>(users.Count);
            foreach(var user in users)
            {
                var identity = new UserIdentity
                {
                    ApplicationUserId = user.Id,
                    SteamId = user.SteamId,
                    SubscriptionType = subscriptionTypes.Single(x=> x.Item1 == user).Item2,
                    DailyMatchesLimit = dailyLimits.Single(x=> x.Item1 == user).Item2       
                };
                identities.Add(identity);
            }

            return identities;
        }
        
        /// <summary>
        /// Read the Headers of a request to see if the Host is api.mentor.gg
        /// Confirming the call is from the Internal and not internal.
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        private bool IsExternalRequest(IHeaderDictionary headers)
        {
            return (headers.SingleOrDefault(x => x.Key == "Host").Value == "api.mentor.gg");
        }

        /// <summary>
        /// Get an Application User's Identity representation.
        /// </summary>
        private async Task<UserIdentity> GetUserIdentityAsync(ApplicationUser user)
        {
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
