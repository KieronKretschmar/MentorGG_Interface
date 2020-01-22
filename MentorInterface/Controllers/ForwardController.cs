using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MentorInterface.Controllers
{
    /// <summary>
    /// Controller to inaccurately duplicate requests from an existing API.
    /// </summary>
    public class ForwardController : ControllerBase
    {

        /// <summary>
        /// Forward a request and enrich the Resonse Headers.
        /// </summary>
        /// <param name="client">HttpClient to forward the message</param>
        /// <param name="requestMessage">Message to send</param>
        /// <returns>Forwared body from the request</returns>
        protected async Task<IActionResult> ForwardHttpRequest(
            HttpClient client, HttpRequestMessage requestMessage)
        {
            try
            {
                var responseMessage = await client.SendAsync(requestMessage);

                //Enrich the current Response header
                //Duplicate the status code and return the response body
                foreach (var item in responseMessage.Content.Headers)
                {
                    Response.Headers.Add(item.Key, item.Value.First());
                }

                string body = await responseMessage.Content.ReadAsStringAsync();
                Response.StatusCode = (int)responseMessage.StatusCode;
                return Content(body);
            }
            catch
            {
                return StatusCode(504);
            }
        }
    }
}
