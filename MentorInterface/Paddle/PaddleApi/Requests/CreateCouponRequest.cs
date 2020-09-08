using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Paddle.PaddleApi
{
    /// <summary>
    /// Data sent to paddle API to create a coupon.
    /// </summary>
    public class CreateCouponRequest : PaddleApiRequest
    {
        /// <summary>
        /// Number of coupons to generate. Not valid if coupon_code is specified.
        /// </summary>
        [JsonProperty(PropertyName = "num_coupons")]
        public int NumberOfCoupons { get; set; }

        /// <summary>
        /// Either product (valid for specified products or subscription plans) or checkout (valid for any checkout).
        /// </summary>
        [JsonProperty(PropertyName = "coupon_type")]
        public string CouponType { get; set; }

        /// <summary>
        /// Comma-separated list of product IDs. Required if coupon_type is product.
        /// </summary>
        [JsonProperty(PropertyName = "product_ids")]
        public string ProductIds { get; set; }

        /// <summary>
        /// Either `flat` or `percentage`.
        /// </summary>
        [JsonProperty(PropertyName = "discount_type")]
        public string DiscountType { get; set; }

        /// <summary>
        /// A currency amount (eg. 10.00) if discount_type is flat, or a percentage amount (eg. 10 for 10%) if discount_type is percentage.
        /// </summary>
        [JsonProperty(PropertyName = "discount_amount")]
        public float DiscountAmount { get; set; }

        /// <summary>
        /// The currency must match the balance currency specified in your account . Required if discount_amount is flat.
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Number of times a coupon can be used in a checkout. This will be set to 999,999 by default, if not specified.
        /// </summary>
        [JsonProperty(PropertyName = "allowed_uses")]
        public int AllowedUses { get; set; }

        /// <summary>
        /// The date (in format YYYY-MM-DD) the coupon is valid until. The coupon will expire on the date at 00:00:00 UTC.
        /// </summary>
        [JsonProperty(PropertyName = "expires")]
        public string Expires { get; set; }

        /// <summary>
        /// If the coupon is used on subscription products, this indicates whether the discount should apply to recurring payments after the initial purchase.
        /// </summary>
        [JsonProperty(PropertyName = "recurring")]
        public int Recurring { get; set; }

        /// <summary>
        /// The name of the coupon group this coupon should be assigned to.
        /// </summary>
        [JsonProperty(PropertyName = "group")]
        public string Group { get; set; }
    }
}
