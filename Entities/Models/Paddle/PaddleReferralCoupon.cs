using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models.Paddle
{
    public class PaddleReferralCoupon
    {
        public int Id { get; set; }

        public long SteamId { get; set; }

        public string Coupon {get; set;}
    }
}