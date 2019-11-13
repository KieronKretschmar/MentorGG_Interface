/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OpenId.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MentorInterface.Controllers
{
    /// <summary>
    /// Controller for Authenticating with Steam OpenID 2.0
    /// </summary>
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        /// <summary>
        /// Create a sign in session with Steam OpenID 2.0
        /// </summary>
        /// <param name="returnUrl">Where to return to once successfully authenticated.</param>
        /// <returns></returns>
        [HttpGet("signin/{returnUrl?}")]
        public IActionResult SteamSignIn(string returnUrl = "/")
        {
            // Authentication provider is defined in ``Startup.cs``
            string authentication_provider = "Steam";
            return Challenge(
                new AuthenticationProperties { RedirectUri = returnUrl },
                authentication_provider);
        }

        /// <summary>
        /// Sign out of the current session.
        /// </summary>
        /// <param name="returnUrl">Where to return to once successfully signed out.</param>
        /// <returns></returns>
        [HttpPost("signout/{returnUrl?}")]
        public IActionResult SignOut(string returnUrl = "/")
        {
            // Instruct the cookies middleware to delete the local cookie created
            // when the user agent is redirected from the external identity provider
            // after a successful authentication flow (e.g Google or Facebook).
            return SignOut(
                new AuthenticationProperties { RedirectUri = returnUrl },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}