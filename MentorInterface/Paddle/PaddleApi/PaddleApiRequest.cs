using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Paddle.PaddleApi
{
    /// <summary>
    /// Base class for each request to the Paddle api. Properties are serialized with the names expected by Paddle Api.
    /// </summary>
    public abstract class PaddleApiRequest
    {
        /// <summary>
        /// Vendor ID. Found at Paddle Dashboard -> Authentication.
        /// </summary>
        [JsonProperty(PropertyName = "vendor_id")]
        public int VendorId { get; set; }

        /// <summary>
        /// Vendor Authentication code. Found at Paddle Dashboard -> Authentication.
        /// </summary>
        [JsonProperty(PropertyName = "vendor_auth_code")]
        public string VendorAuthCode { get; set; }
    }
}
