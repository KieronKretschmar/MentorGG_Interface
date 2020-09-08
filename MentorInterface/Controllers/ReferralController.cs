using System.Threading.Tasks;
using MentorInterface.Models;
using MentorInterface.Paddle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MentorInterface.Controllers
{
    /// <summary>
    /// Controller responsible for Referrals
    /// </summary>
    [AllowAnonymous]
    [Route("referrals")]
    public class ReferralController : Controller
    {
        private readonly ILogger<ReferralController> _logger;

        private readonly IPaddleApiCommunicator _paddleApi;

        /// <summary>
        /// Default Contructor
        /// </summary>
        /// <param name="logger"></param>
        public ReferralController(
        IPaddleApiCommunicator paddleApi,
        ILogger<ReferralController> logger)
        {
            _paddleApi = paddleApi;
            _logger = logger;  
        }

        /// <summary>
        /// Return a Paddle coupon to the User, If User has referred enough new users.
        /// </summary>
        /// <returns></returns>
        [HttpGet("coupon")]
        public async Task<ActionResult<ReferralCoupon>> GetCouponAsync()
        {
            string coupon = await _paddleApi.CreateReferralCouponAsync();
            return new ReferralCoupon
            {
                Coupon = coupon,
            };
        }
    }
}