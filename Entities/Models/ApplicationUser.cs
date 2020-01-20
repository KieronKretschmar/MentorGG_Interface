using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{

    /// <summary>
    /// Helper factory to create Application Users.
    /// </summary>
    public static class ApplicationUserFactory
    {
        /// <summary>
        /// Create a new user from a Steam community URL.
        /// pattern: "steamcommunity.com/openid/id/76561198004197138"
        /// </summary>
        /// <param name="community_url"></param>
        public static ApplicationUser FromCommunityUrl(string community_url)
        {
            var success = long.TryParse(community_url.Split('/').Last(), out long steamId);
            if (success) {
                return new ApplicationUser(steamId);
            }
            else
            {
                throw new ArgumentException($"Invalid / Malformed - Community Url provided: [ {community_url} ]");
            }

        }

        /// <summary>
        /// Helper function to create a new Application User from a Steam Id.
        ///
        /// Exists to provide a consistent factory interface.
        /// </summary>
        /// <param name="steamId"></param>
        /// <returns></returns>
        public static ApplicationUser FromSteamId(long steamId)
        {
            return new ApplicationUser(steamId);
        }
    }

    /// <summary>
    /// Holds information regarding a user of this application.
    /// </summary>
    public class ApplicationUser : IdentityUser<int>
    {
        /// <summary>
        /// SteamId
        /// </summary>
        public long SteamId { get; set; }

        /// <summary>
        /// Time of Registration
        /// </summary>
        public DateTime Registration { get; set; }

        /// <summary>
        /// Create a user from a SteamId.
        /// </summary>
        public ApplicationUser(long steamId) : base()
        {
            SteamId = steamId;
            UserName = steamId.ToString();
            Registration = DateTime.UtcNow;
        }

    }
}