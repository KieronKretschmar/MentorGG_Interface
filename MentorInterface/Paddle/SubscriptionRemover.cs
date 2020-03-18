using Database;
using Entities.Models;
using Entities.Models.Paddle;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MentorInterface.Paddle
{
    /// <summary>
    /// Offers methods to remove subscriptions cleanly (including roles).
    /// </summary>
    public class SubscriptionRemover
    {
        private readonly ILogger<SubscriptionRemover> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationContext _applicationContext;

        public SubscriptionRemover(ILogger<SubscriptionRemover> logger, UserManager<ApplicationUser> userManager, ApplicationContext applicationContext)
        {
            _logger = logger;
            _userManager = userManager;
            _applicationContext = applicationContext;
        }


        /// <summary>
        /// Queries the database for expired subscriptions and removes them accordingly.
        /// </summary>
        /// <returns></returns>
        public async Task RemoveAllExpiredSubscriptions()
        {
            var expiredSubscriptions = _applicationContext.PaddleSubscription.Where(x => x.ExpirationTime < DateTime.Now);
            foreach (var subscription in expiredSubscriptions)
            {
                await RemoveSubscription(subscription);
            }
        }

        /// <summary>
        /// Removes a subscription along with the user's Roles he gained through this subscription only.
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        private async Task RemoveSubscription(PaddleSubscription subscription)
        {
            _logger.LogInformation($"Removing Subscription#{subscription.SubscriptionId} from User#{subscription.ApplicationUserId}");

            var appUser = await _applicationContext.Users.FindAsync(subscription.ApplicationUserId);

            // Assuming that included tables stay small and the .Include's won't lead to carthesian explosion.
            var userSubscriptions = _applicationContext.PaddleSubscription
                .Include(x => x.PaddlePlan)
                .ThenInclude(x => x.PaddlePlanRoles)
                .ThenInclude(x=>x.Role)
                .Where(x => x.ApplicationUserId == appUser.Id)
                .ToList();

            // Determine roles the user had because of this subscription,
            // excluding roles that the user does not also have from other subscriptions
            var rolesFromOtherSubscriptions = userSubscriptions
                .Where(x => x.SubscriptionId != subscription.SubscriptionId)
                .SelectMany(x => x.PaddlePlan.PaddlePlanRoles.Select(y => y.Role.Name))
                .Distinct()
                .ToList();

            var rolesToRemove = userSubscriptions
                .Single(x => x.SubscriptionId == subscription.SubscriptionId).PaddlePlan.PaddlePlanRoles
                .Select(x => x.Role.Name)
                .Except(rolesFromOtherSubscriptions);

            // Remove all Roles from the user that are not supplied by any other role
            await _userManager.RemoveFromRolesAsync(subscription.User, rolesToRemove);

            // Remove this subscription from database
            _applicationContext.PaddleSubscription.Remove(subscription);
            await _applicationContext.SaveChangesAsync();

            _logger.LogInformation($"Successfully removed Subscription#{subscription.SubscriptionId} from User#{subscription.ApplicationUserId}");
        }
    }
}
