using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MentorInterface.Controllers.AutomaticUpload
{
    /// <summary>
    /// Communicator for managing user connections for the SharingCodeGatherer service.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/automatic-upload/connections/valve")]
    public class SharingCodeGathererController : ControllerBase
    {
        /// <summary>
        /// Http Client Factory
        /// </summary>
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Create the controller and inject the HTTPClient factory.
        /// </summary>
        public SharingCodeGathererController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Query SharingCodeGatherer for all information pertaining to the current user.
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
        /// Query SharingCodeGather to create a connection with the Valve API for the current user.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Connections" })]
        public IActionResult ConnectUser(string steamAuthToken, string lastKnownSharingCode)
        {
            return StatusCode(501);
        }

        /// <summary>
        /// Query SharingCodeGather to create remove the connection with the Vavle API for the current user.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [SwaggerOperation(Tags = new[] { "Connections" })]
        public IActionResult DisconnectUser()
        {
            return StatusCode(501);
        }

    }
}