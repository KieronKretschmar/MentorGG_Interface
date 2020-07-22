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

        private readonly List<SubscriptionType> availableSubscriptionTypes = new List<SubscriptionType>
        {
            SubscriptionType.Premium,
            SubscriptionType.Ultimate
        };

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
        [HttpGet]
        public async Task<SubscriptionsModel> GetSubscriptionsAsync()
        {
            var model = new SubscriptionsModel();

            var user = await _userManager.GetUserAsync(User);
            if(user != null)
            {
                // explicitly load PaddleSubscriptions for this user
                _applicationContext.Entry(user).Collection(x => x.PaddleSubscriptions).Load();

                model.ActiveSubscription = _applicationContext.PaddleSubscription
                    .Where(x => x.ApplicationUserId == user.Id)
                    .Include(x => x.PaddlePlan)
                    .Select(x => new PaddleSubscriptionModel(x.PaddlePlan.SubscriptionType, x))
                    .SingleOrDefault();
            }

            // Get all PaddlePlans available to the user, grouped by SubscriptionType
            model.AvailableSubscriptions = _applicationContext.PaddlePlan
                .Where(x => availableSubscriptionTypes.Contains(x.SubscriptionType))
                .ToList()
                .GroupBy(x => x.SubscriptionType)
                .Select(x => new AvailableSubscription(x.Key, x.ToList()))
                .ToList();

            return model;
        }
    }
}
