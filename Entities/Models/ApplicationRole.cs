using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    /// <summary>
    /// Add profile data for application users by adding properties to the ApplicationUser class
    /// </summary>
    public class ApplicationRole : IdentityRole<int>
    {
        /// <summary>
        /// Indicate if the user is Premium
        /// </summary>
        public bool Premium { get; set; }
    }
}