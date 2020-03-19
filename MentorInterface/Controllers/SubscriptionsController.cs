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
using Database;
using Microsoft.EntityFrameworkCore;
using Entities;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MentorInterface.Controllers
{
    /// <summary>
    /// Controller for providing the webapp with data regarding a user's subscriptions.
    /// </summary>
    [Route("subscriptions")]
    public class SubscriptionsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationContext _applicationContext;
        private readonly ILogger<SubscriptionsController> _logger;

        public SubscriptionsController(
            UserManager<ApplicationUser> userManager,
            ApplicationContext applicationContext,
            ILogger<SubscriptionsController> logger
            )
        {
            _userManager = userManager;
            _applicationContext = applicationContext;
            _logger = logger;
        }


        /// <summary>
        /// Return the currently logged in User's active and available subscriptions.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<SubscriptionsModel> GetSubscriptionsAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            // explicitly load PaddleSubscriptions for this user
            _applicationContext.Entry(user).Collection(x => x.PaddleSubscriptions).Load();

            var activeSubscription = _applicationContext.PaddleSubscription
                .Where(x => x.ApplicationUserId == user.Id)
                .Include(x => x.PaddlePlan)
                .Select(x => new PaddleSubscriptionModel(x.PaddlePlan.SubscriptionType, x))
                .SingleOrDefault();

            var availableSubscriptionTypes = GetAvailableSubscriptions(activeSubscription?.SubscriptionType ?? SubscriptionType.Free);

            // Get all PaddlePlans available to the user, grouped by SubscriptionType
            var availableSubscriptions = _applicationContext.PaddlePlan
                .Where(x => availableSubscriptionTypes.Contains(x.SubscriptionType))
                .ToList()
                .GroupBy(x => x.SubscriptionType)
                .Select(x => new AvailableSubscription(x.Key, x.ToList()))
                .ToList();

            return new SubscriptionsModel
            {
                ActiveSubscription = activeSubscription,
                AvailableSubscriptions = availableSubscriptions
            };
        }

        private List<SubscriptionType> GetAvailableSubscriptions(SubscriptionType currentSubscriptionType)
        {
            var res = new List<SubscriptionType>();
            switch (currentSubscriptionType)
            {
                case SubscriptionType.Free:
                    return new List<SubscriptionType> { SubscriptionType.Premium, SubscriptionType.Ultimate };
                case SubscriptionType.Premium:
                    return new List<SubscriptionType> { SubscriptionType.Ultimate };
                case SubscriptionType.Ultimate:
                    return new List<SubscriptionType>();
                default:
                    throw new Exception($"Could not determine AvailableSubscriptions for SubscriptionType [ {currentSubscriptionType} ]");
                    break;
            }
        }
    }
}