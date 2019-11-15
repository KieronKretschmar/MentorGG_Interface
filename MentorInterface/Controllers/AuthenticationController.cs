/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OpenId.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MentorInterface.Authentication;
using MentorInterface.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MentorInterface.Controllers
{
    /// <summary>
    /// Controller for Authenticating with Steam OpenID 2.0
    /// </summary>
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AuthenticationController> logger,
            IAuthenticationService authenticationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("login/{returnUrl?}")]
        public IActionResult Login(string returnUrl = "/")
        {
            var props = new AuthenticationProperties {
                
                RedirectUri = Url.Action("SteamLoginCallback", "Authentication", new { ReturnUrl = returnUrl}),
            };
            return new ChallengeResult(MentorAuthenticationSchemes.STEAM, props);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        /// 
        /// Ignore this Endpoint from Swagger Documentation.
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> SteamLoginCallback(string returnUrl)
        {
            // Get the Authentication Result
            AuthenticateResult result = await _authenticationService
                .AuthenticateAsync(HttpContext, MentorAuthenticationSchemes.STEAM);

            //AuthenticateResult id_result = await _authenticationService
            //    .AuthenticateAsync(HttpContext, IdentityConstants.ExternalScheme);

            ClaimsIdentity result_identity = result.Principal.Identity as ClaimsIdentity;
            var result_claim = result_identity.Claims.First();

            // Get the steam community URL
            // steamcommunity.com/openid/id/76561198004197138
            var steam_claim = result_claim.Value;

            // Extract the SteamID
            long steamID;
            try
            {
                long.TryParse(steam_claim.Split('/').Last(), out steamID);
                _logger.LogInformation($"Successful SteamID Retreival {steamID}");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to retreive SteamID");
                return StatusCode(500);
            }

            

            // Attempt to Login

            // Create User
            var user = new ApplicationUser(steamID);
            var create_result = await _userManager.CreateAsync(user);

            if (create_result.Succeeded)
            {
                var login_result = _userManager.AddLoginAsync(user, new UserLoginInfo("Steam", steam_claim, user.SteamID.ToString()));
                if (login_result.IsCompletedSuccessfully)
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);
                }
            }

            return Redirect(returnUrl);
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