using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MentorInterface.Controllers.AutomaticUpload
{
    /// <summary>
    /// Connection status controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/automatic-upload/connections")]
    public class ConnectionsController : ControllerBase
    {
        /// <summary>
        /// Query all automatic upload gatherers and return their respective connection status.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IActionResult Status()
        {
            return StatusCode(501);
        }

    }
}