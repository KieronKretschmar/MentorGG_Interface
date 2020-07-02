using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Authentication
{
    /// <summary>
    /// A Collection of Supported Authentication Schemes.
    /// </summary>
    public static class MentorAuthenticationSchemes
    {
        /// <summary>
        /// Steam Open ID Provider.
        /// </summary>
        public const string STEAM = "OpenID.Steam";

        /// <summary>
        /// Mentor Issued JWT Token.
        /// </summary>
        public const string JWT = "JWT";
    }
}
