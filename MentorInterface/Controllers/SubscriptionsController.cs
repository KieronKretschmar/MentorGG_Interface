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

        public SubscriptionsController(
            UserManager<ApplicationUser> userManager,
            ApplicationContext applicationContext
            )
        {
            this._userManager = userManager;
            _applicationContext = applicationContext;
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

            var activeSubscriptions = user.PaddleSubscriptions
                .Select(x => new PaddleSubscriptionModel(x))
                .ToList();

            var availableSubscriptions = new List<AvailableSubscription>();
            var subscriptionTypes = Subscriptions.All.Select(x => x.Type);
            foreach (var type in subscriptionTypes)
            {
                var paddlePlans = _applicationContext.PaddlePlan
                    .Where(x => x.SubscriptionType == type)
                    .ToList();
                var availableSubscription = new AvailableSubscription(type, paddlePlans);
                availableSubscriptions.Add(availableSubscription);
            }


            return new SubscriptionsModel
            {
                ActiveSubscriptions = activeSubscriptions,
                AvailableSubscriptions = availableSubscriptions
            };
        }
    }
}
