﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MentorInterface.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MentorInterface.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/")]
    public class BrowserExtensionController : ForwardController
    {
        private readonly HttpClient _client;

        public BrowserExtensionController(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient(ConnectedServices.DemoCentral);
        }

        [HttpPost("extension-upload")]
        public async Task ForwardMatchesFromExtensions()
        {
            var message = Request;
            string requestUrl = "v1/public/extensionupload-valve";

            string data;
            using (var reader = new StreamReader(message.Body))
            {
                data = await reader.ReadToEndAsync();
            }

            //Remove all line breaks from the input as these are not permitted in urls
            data = Regex.Replace(data,"\n|\t","");

            var forwardedResponse = new HttpRequestMessage(
                HttpMethod.Post, $"{requestUrl}?data={data}"
                 );

            await ForwardHttpRequest(_client, forwardedResponse);
        }
    }
}