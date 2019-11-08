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
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        // GET: <controller>/signin
        [HttpGet("signin/{returnUrl?}")]
        public IActionResult SteamSignIn(string returnUrl = "/")
        {
            // Authentication provider is defined in ``Startup.cs``
            string authentication_provider = "Steam";
            return Challenge(
                new AuthenticationProperties { RedirectUri = returnUrl },
                authentication_provider);
        }

        // POST <controller>/signout
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