using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MentorInterface.Controllers
{
    /// <summary>
    /// Controller for Verifying Authentication
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class VerifyController : Controller
    {
        /// <summary>
        /// Verify that the caller is Authenticated.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return BadRequest();
            }
            return Content(
                $"Name: {identity.Name}, AuthType: {identity.AuthenticationType}, Claims: {identity.Claims}");
        }
    }
}
