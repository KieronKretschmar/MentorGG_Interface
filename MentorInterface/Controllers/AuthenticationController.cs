/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OpenId.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
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
        [HttpGet("steam_signin/{returnUrl?}")]
        public IActionResult SteamSignIn(string returnUrl = "/")
        {
            var props = new AuthenticationProperties {
                RedirectUri = Url.Action("SteamLoginCallback", "Authentication",
                new { ReturnUrl = returnUrl}),
            };
            return new ChallengeResult(MentorAuthenticationSchemes.STEAM, props);
        }

        /// <summary>
        /// Sign out of the current session.
        /// </summary>
        /// <param name="returnUrl">Where to return to once successfully signed out.</param>
        /// <returns></returns>
        [HttpPost("steam_signout/{returnUrl?}")]
        public IActionResult SteamSignOut(string returnUrl = "/")
        {
            // Instruct the cookies middleware to delete the local cookie created
            // when the user agent is redirected from the external identity provider
            // after a successful authentication flow (e.g Google or Facebook).
            return SignOut(
                new AuthenticationProperties { RedirectUri = returnUrl },
                MentorAuthenticationSchemes.STEAM);
        }

        /// Ignore this Endpoint from Swagger Documentation.
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> SteamLoginCallback(string returnUrl)
        {
            // Get the Authentication Result
            AuthenticateResult result = await _authenticationService
                .AuthenticateAsync(HttpContext, MentorAuthenticationSchemes.STEAM);

            ClaimsIdentity result_identity = result.Principal.Identity as ClaimsIdentity;
            Claim steam_claim = result_identity.Claims.First();

            var claim_users = await _userManager.GetUsersForClaimAsync(steam_claim);
            if (claim_users.Any())
            {
                ApplicationUser existing_user = claim_users.First();
                try
                {
                    // This call will gather all claims regarding this user and attempt to sign in.
                    await _signInManager.SignInAsync(existing_user, isPersistent: true);
                    return Redirect(returnUrl);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "SignIn failed for a ApplicationUser with an attached Claim");
                    return StatusCode(500);
                }
            }

            // Create a new ApplicationUser
            ApplicationUser new_user;
            try
            {
                new_user = new ApplicationUser(community_url: steam_claim.Value);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to create new user from Steam Community URL");
                return StatusCode(500);
            }

            // Attempt to add the new user to the connected data store.
            var create_new_user_result = await _userManager.CreateAsync(new_user);
            if (create_new_user_result.Succeeded)
            {
                // Assign the steam claim to the user
                var claim_result = await _userManager.AddClaimAsync(new_user, steam_claim);
                if (claim_result.Succeeded)
                {
                    await _signInManager.SignInAsync(new_user, isPersistent: true);
                }
            }
            else
            {
                string errors = create_new_user_result.Errors.ToString();
                _logger.LogError("Error creating User: " + errors);
            }

            return Redirect(returnUrl);
        }

    }
}