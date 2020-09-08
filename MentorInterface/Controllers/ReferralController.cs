using System.Linq;
using System.Threading.Tasks;
using Database;
using Entities.Models;
using MentorInterface.Models;
using MentorInterface.Paddle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MentorInterface.Controllers
{
    /// <summary>
    /// Controller responsible for Referrals
    /// </summary>
    [Authorize]
    [Route("referrals")]
    public class ReferralController : Controller
    {
        private readonly ILogger<ReferralController> _logger;

        private readonly ApplicationContext _applicationContext;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IPaddleApiCommunicator _paddleApi;
        

        /// <summary>
        /// Default Contructor
        /// </summary>
        public ReferralController(
        IPaddleApiCommunicator paddleApi,
        ApplicationContext applicationContext,
        UserManager<ApplicationUser> userManager,
        ILogger<ReferralController> logger)
        {
            _paddleApi = paddleApi;
            _applicationContext = applicationContext;
            _userManager = userManager;
            _logger = logger;  
        }

        /// <summary>
        /// Return a Paddle coupon to the User, If User has referred enough new users.
        /// </summary>
        /// <returns></returns>
        [HttpGet("coupon")]
        public async Task<ActionResult<ReferralCoupon>> GetCouponAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            int referrals = _applicationContext.Users.Where(x=> x.RefererSteamId == currentUser.SteamId).Count();

            _logger.LogInformation($"Current User [ {currentUser.SteamId} ] has referred [ {referrals } ] users.");

            if(referrals >= 4)
            {
                return new ReferralCoupon
                {
                    Coupon = await _paddleApi.CreateReferralCouponAsync(),
                    Referrals = referrals,
                };
            }

            return new ReferralCoupon
            {
                Referrals = referrals
            };
        }
    }
}