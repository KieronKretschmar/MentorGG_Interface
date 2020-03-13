using Entities.Models.Paddle;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Entities.Models
{
    /// <summary>
    /// Add profile data for application users by adding properties to the ApplicationUser class
    /// </summary>
    public class ApplicationRole : IdentityRole<int>
    {
        /// <summary>
        /// The number of matches for each day users with this subscription may see.
        /// </summary>
        public int DailyMatchesLimit { get; set; } = 0;

        /// <summary>
        /// Navigational Property
        /// </summary>
        public virtual ICollection<PaddlePlanRole> PaddlePlanRoles { get; set; }
        public ApplicationRole() { }
        public ApplicationRole(string roleName, int dailyMatchesLimit) : base(roleName)
        {
            DailyMatchesLimit = dailyMatchesLimit;
        }
    }
}