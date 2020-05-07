using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MentorInterface.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MentorInterface.Controllers
{
    [Route("v{version:apiVersion}/")]
    [ApiController]
    public class DownloadController : ForwardController
    {
        private readonly IHttpClientFactory _clientFactory;

        public DownloadController(
            IHttpClientFactory clientFactory
            )
        {
            _clientFactory = clientFactory;
        }


        /// <summary>
        /// Get blobUrl of demo file.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Premium,Ultimate")]
        [HttpGet("match/{matchId}/download-url")]
        public async Task<IActionResult> DownloadUrlAsync(long matchId)
        {
            var client = _clientFactory.CreateClient(ConnectedServices.DemoCentral);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get,
                $"/v1/public/match/{matchId}/download-url");

            return await ForwardHttpRequest(client, message);
        }
    }
}