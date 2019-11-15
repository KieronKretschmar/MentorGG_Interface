/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OpenId.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using System.Threading.Tasks;
using MentorInterface.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace MentorInterface.Controllers
{
    /// <summary>
    /// Controller for Authenticating with Steam OpenID 2.0
    /// </summary>
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("login/{returnUrl?}")]
        public IActionResult Login(string returnUrl = "/")
        {
            var props = new AuthenticationProperties {
                
                RedirectUri = Url.Action("ExternalLoginCallback", "Authentication", new { ReturnUrl = returnUrl}),
            };
            return new ChallengeResult("Steam", props);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            AuthenticateResult result = await HttpContext.RequestServices.GetRequiredService<IAuthenticationService>().AuthenticateAsync(HttpContext, "OpenID.Steam");
            System.Diagnostics.Debug.WriteLine(result.Succeeded);
            System.Diagnostics.Debug.WriteLine(result.Ticket.Properties.Items);
            System.Diagnostics.Debug.WriteLine(result.Ticket.AuthenticationScheme);
            System.Diagnostics.Debug.WriteLine(result.Ticket.Principal.Claims);
            System.Diagnostics.Debug.WriteLine(result.Ticket.Properties.Parameters);
            System.Diagnostics.Debug.WriteLine(result.Principal.Claims);
            System.Diagnostics.Debug.WriteLine(result.Principal.Identity);

            AuthenticateResult result = await HttpContext.RequestServices.GetRequiredService<OpenIdAuthenticationExtensions>()

            foreach (var claim in User.Claims)
            {
                System.Diagnostics.Debug.WriteLine(claim);
            }
            //var claimsPrincipal = await HttpContext.Authentication.AuthenticateAsync("ExternalCookie");
            var identity = User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return BadRequest();
            }
            return Content("");
        }

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