using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MentorInterface.Data
{
    /// <summary>
    /// Add profile data for application users by adding properties to the ApplicationUser class
    /// </summary>
    public class ApplicationUser : IdentityUser<int>
    {
        /// <summary>
        /// SteamID
        /// </summary>
        public string SteamID { get; set; }
    }
}