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
        private readonly ILogger<AuthenticationController> logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

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
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
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
                nameof(SteamLoginCallback),
                controllerName,
                new { ReturnUrl = returnUrl });
            var props = signInManager.ConfigureExternalAuthenticationProperties(
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
        public async Task<ActionResult> SteamLoginCallback(string returnUrl = "/")
        {
            var loginInfo = await signInManager.GetExternalLoginInfoAsync();

            var result = await signInManager.ExternalLoginSignInAsync(
                loginInfo.LoginProvider,
                loginInfo.ProviderKey,
                isPersistent: false);

            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return await RegisterSteamUser(loginInfo, returnUrl);
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
        private async Task<ActionResult> RegisterSteamUser(ExternalLoginInfo loginInfo, string returnUrl = "/")
        {
            ClaimsIdentity result_identity = loginInfo.Principal.Identity as ClaimsIdentity;

            // Explictly return the corrent claim associated with the SteamId.
            Claim steam_claim = result_identity.Claims.Single(o =>
            {
                return o.Value.Contains("openid/id");
            });

            // Create a new ApplicationUser
            ApplicationUser new_user;
            try
            {
                new_user = ApplicationUserFactory.FromCommunityUrl(community_url: steam_claim.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to create new user from Steam Community URL");
                return StatusCode(500);
            }

            // Attempt to add the new user to the connected data store.
            var create_new_user_result = await userManager.CreateAsync(new_user);
            if (create_new_user_result.Succeeded)
            {
                await userManager.AddLoginAsync(new_user, loginInfo);
                await signInManager.SignInAsync(new_user, isPersistent: true);
            }
            else
            {
                string errors = create_new_user_result.Errors.ToString();
                logger.LogError("Error creating User: " + errors);
            }

            return Redirect(returnUrl);
        }

        #endregion
    }

}
