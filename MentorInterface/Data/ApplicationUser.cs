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
            UserName = steamID.ToString();
        }

        /// <summary>
        /// Create a user from a Steam community URL.
        /// pattern: "steamcommunity.com/openid/id/76561198004197138"
        /// </summary>
        /// <param name="community_url"></param>
        public ApplicationUser(string community_url) : base()
        {
            long steamID;
            long.TryParse(community_url.Split('/').Last(), out steamID);
            SteamID = steamID;
            UserName = steamID.ToString();
        }
    }
}