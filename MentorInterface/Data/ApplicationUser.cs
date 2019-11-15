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
        public long SteamID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ApplicationUser(long steamID) : base() 
        {
            SteamID = steamID;
            UserName = "test";
        }
    }
}