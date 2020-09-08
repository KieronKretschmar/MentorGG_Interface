using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MentorInterface.Paddle.PaddleApi.Responses
{
    public class CreateCouponResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "response")]
        public CouponCodeResponse CouponCodeResponse { get; set; }

        public string FirstCoupon()
        {
            return CouponCodeResponse.CouponCodes.First();
        }
    }

    public class CouponCodeResponse
    {
        [JsonProperty(PropertyName = "coupon_codes")]       
        public string[] CouponCodes { get; set; }
    }
}