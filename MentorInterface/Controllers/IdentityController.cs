﻿using System;
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

        public IdentityController(
            UserManager<ApplicationUser> userManager,
            IRoleHelper roleHelper,
            ApplicationContext applicationContext
            )
        {
            _userManager = userManager;
            _roleHelper = roleHelper;
            _applicationContext = applicationContext;
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
        /// Return Identity of any known SteamId.
        /// </summary>
        [HttpGet("{steamId}")]
        public async Task<UserIdentity> GetIdentityFromSteamIdAsync(long steamId)
        {
            var user =  _applicationContext.Users.SingleOrDefault(x => x.SteamId == steamId);
            if (user != null)
            {
                return await GetUserIdentityAsync(user);
            }
            else
            {
                Response.StatusCode = 404;
                return null;
            }
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
