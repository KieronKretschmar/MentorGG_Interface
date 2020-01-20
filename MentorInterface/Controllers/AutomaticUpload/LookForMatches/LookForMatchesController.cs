﻿using System;
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
    /// Look for matches controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/automatic-upload/")]
    public class LookForMatchesController : ControllerBase
    {
        /// <summary>
        /// Http Client Factory
        /// </summary>
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Create the controller and inject the HTTPClient factory.
        /// </summary>
        public LookForMatchesController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Query FaceItMatchGatherer to look for matches.
        /// </summary>
        /// <returns></returns>

        [Authorize]
        [HttpPost("faceit/look")]
        public IActionResult FaceIt()
        {
            return StatusCode(501);
        }

        /// <summary>
        /// Query SharingCodeGatherer to look for matches.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("valve/look")]
        public IActionResult Valve()
        {
            return StatusCode(501);
        }

    }
}