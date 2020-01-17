using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentorInterface.Controllers.AutomaticUpload
{
    /// <summary>
    /// Communicator for the managing user connections for the FaceItMatchGatherer service.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/automatic-upload/connections/faceit")]
    public class FaceItMatchGathererController : ControllerBase
    {
        /// <summary>
        /// Query FaceItMatchGatherer for all information pertaining to the current user.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Connections" })]
        public IActionResult Status()
        {
            return StatusCode(501);
        }

        /// <summary>
        /// Query FaceItMatchGatherer to establish a connection with the FaceIt API for the current user.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Connections" })]
        public IActionResult Connect(string code)
        {
            return StatusCode(501);
        }

        /// <summary>
        /// Query FaceItMatchGatherer to remove the connection with the FaceIt API for the current user.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Connections" })]
        public IActionResult Disconnect()
        {
            return StatusCode(501);
        }

    }
}