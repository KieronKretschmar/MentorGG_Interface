using System;
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

        private string controllerName => this.ControllerContext.RouteData.Values["controller"].ToString();

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
        /// <param name="returnUrl">Return Url</param>
        /// <returns></returns>
        [HttpGet("signin/steam")]
        public IActionResult SteamSignIn(string returnUrl = "/")
        {
            var redirectUrl = Url.Action(
                nameof(SteamLoginCallbackAsync),
                controllerName,
                new { ReturnUrl = returnUrl });
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
        public IActionResult SignOut(string returnUrl = "/")
        {
            return SignOut(
                new AuthenticationProperties { RedirectUri = returnUrl },
                IdentityConstants.ApplicationScheme);
        }

        /// <summary>
        /// Steam Login callback, log in existing users and register new users.
        /// </summary>
        /// <param name="returnUrl">Return Url</param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("callback/steam")]
        public async Task<ActionResult> SteamLoginCallbackAsync(string returnUrl = "/")
        {
            var loginInfo = await _signInManager.GetExternalLoginInfoAsync();

            var result = await _signInManager.ExternalLoginSignInAsync(
                loginInfo.LoginProvider,
                loginInfo.ProviderKey,
                isPersistent: false);

            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return await RegisterSteamUserAsync(loginInfo, returnUrl);
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
        private async Task<ActionResult> RegisterSteamUserAsync(ExternalLoginInfo loginInfo, string returnUrl = "/")
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
