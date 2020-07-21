using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using MentorInterface.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

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
        private readonly IJsonWebTokenGenerator _jsonWebTokenGenerator;

        #region Public Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJsonWebTokenGenerator jsonWebTokenGenerator,
            ILogger<AuthenticationController> logger)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._jsonWebTokenGenerator = jsonWebTokenGenerator;
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
            var redirectUrl = $"/authentication/callback/steam?returnUrl={returnUrl}";
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
        /// <param name="returnUrl">Return Url</param>
        /// <returns></returns>
        [Authorize("Steam")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("callback/steam")]
        public async Task<ActionResult> SteamLoginCallbackAsync(string returnUrl = "/")
        {
            ClaimsIdentity identity = User.Identity as ClaimsIdentity;

            foreach (var claim in identity.Claims)
            {
                _logger.LogInformation($"{claim.Type}:{claim.Value}");
            }

            // Explictly return the corrent claim associated with the SteamId.
            Claim steamClaim = identity.Claims.Single(o =>
            {
                return o.Type == "steamId";
            });

            // Try find an existing User
            var users = await _userManager.GetUsersForClaimAsync(steamClaim);
            ApplicationUser user = users.FirstOrDefault();

            if(user == null)
            {
                user = await RegisterSteamUserAsync(steamClaim);
            }

            var tokenString = _jsonWebTokenGenerator.GenerateJSONWebToken(user);
            _logger.LogCritical(tokenString);
            return Ok(new {token = tokenString});

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Register and sign in a new User, from Steam.
        /// </summary>
        /// <returns></returns>
        private async Task<ApplicationUser> RegisterSteamUserAsync(Claim steamClaim)
        {
            long steamId;
            bool success = long.TryParse(steamClaim.Value, out steamId);
            if(!success)
            {
                throw new ArgumentException("SteamClaim does not contain a valid SteamId!");
            }

            // Create a new ApplicationUser
            ApplicationUser newUser = new ApplicationUser(steamId);

            // Attempt to add the new user to the connected data store.
            var newUserCreationResult = await _userManager.CreateAsync(newUser);

            if (newUserCreationResult.Succeeded)
            {
                await _userManager.AddClaimAsync(newUser, steamClaim);
            }
            else
            {
                string errors = newUserCreationResult.Errors.ToString();
                _logger.LogError("Error creating User: " + errors);
            }

            return newUser;
        }

        #endregion
    }

}
