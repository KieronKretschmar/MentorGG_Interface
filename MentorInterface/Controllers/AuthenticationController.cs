using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Entities.Models;
using MentorInterface.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MentorInterface.Controllers
{
    /// <summary>
    /// Controller for Authenticating with Steam OpenID 2.0
    /// </summary>
    [Route("authentication")]
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        #region Public Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AuthenticationController> logger)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._logger = logger;
        }

        /// <summary>
        /// Sign in with Steams OpenID service.
        /// </summary>
        [HttpGet("signin/steam")]
        public IActionResult SteamSignIn(string returnUrl = "/", string referrer = "")
        {
            var redirectUrl = $"/authentication/callback/steam?returnUrl={returnUrl}";
            if(!string.IsNullOrEmpty(referrer))
            {
                 redirectUrl += $"&referrer={referrer}";
            }

            var props = _signInManager.ConfigureExternalAuthenticationProperties(
                provider: MentorAuthenticationSchemes.STEAM,
                redirectUrl: redirectUrl);

            return new ChallengeResult(MentorAuthenticationSchemes.STEAM, props);
        }

        /// <summary>
        /// Sign out of the current session.
        /// </summary>
        /// <param name="returnUrl">Return Url</param>
        /// <returns></returns>
        [HttpGet("signout")]
        public async Task<IActionResult> SignOut(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

        /// <summary>
        /// Steam Login callback, log in existing users and register new users.
        /// </summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("callback/steam")]
        public async Task<ActionResult> SteamLoginCallbackAsync(string returnUrl = "/", string referrer = "")
        {
            var loginInfo = await _signInManager.GetExternalLoginInfoAsync();

            var signInAttempt = await _signInManager.ExternalLoginSignInAsync(
                loginInfo.LoginProvider,
                loginInfo.ProviderKey,
                isPersistent: true);

            if (signInAttempt.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return await RegisterSteamUserAsync(loginInfo, returnUrl, referrer);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Register and sign in a new User, Logged in from Steam.
        /// </summary>
        /// <param name="loginInfo">Steam Login Info</param>
        /// <param name="returnUrl">Return Url</param>
        /// <returns></returns>
        private async Task<ActionResult> RegisterSteamUserAsync(ExternalLoginInfo loginInfo, string returnUrl = "/", string referrer = "")
        {
            ClaimsIdentity loginIdentity = loginInfo.Principal.Identity as ClaimsIdentity;

            // Explictly return the corrent claim associated with the SteamId.
            Claim steamClaim = loginIdentity.Claims.Single(o =>
            {
                return o.Value.Contains("openid/id");
            });

            // Create a new ApplicationUser
            ApplicationUser newUser;
            try
            {
                newUser = ApplicationUserFactory.FromCommunityUrl(community_url: steamClaim.Value);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to create new user from Steam Community URL");
                return StatusCode(500);
            }

            // Attach the Referer, if present

            if (!string.IsNullOrEmpty(referrer))
            {
                long steamId;
                bool success = long.TryParse(referrer, out steamId);
                if (success)
                {
                    newUser.RefererSteamId = steamId;
                    _logger.LogInformation($"New user [ {newUser.SteamId} ] was referred by [ {referrer} ]");
                }
                else
                {
                    _logger.LogError($"Failed to assign referrer [ {referrer } ] to new ApplicationUser [ {newUser.SteamId} ]");
                }
            }

            // Attempt to add the new user to the connected data store.
            var newUserCreationResult = await _userManager.CreateAsync(newUser);
            if (newUserCreationResult.Succeeded)
            {
                await _userManager.AddLoginAsync(newUser, loginInfo);
                await _signInManager.SignInAsync(newUser, isPersistent: true);
            }
            else
            {
                string errors = newUserCreationResult.Errors.ToString();
                _logger.LogError("Error creating User: " + errors);
            }

            return Redirect(returnUrl);
        }

        #endregion
    }

}
