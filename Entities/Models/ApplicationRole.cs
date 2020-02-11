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
        /// Navigational Property
        /// </summary>
        public ICollection<PaddlePlan> PaddlePlan { get; set; }

        public ApplicationRole() { }
        public ApplicationRole(string roleName) : base(roleName) { }
    }
}