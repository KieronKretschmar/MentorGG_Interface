namespace Entities.Models.Paddle
{
    public interface IPaddleUser
    {
        int UserId { get; set; }

        string Email { get; set; }

        bool MarketingConsent { get; set; }

        int SubscriptionId { get; set; }

        int SubscriptionPlanId { get; set; }

        string Status { get; set; }

        string CancelUrl { get; set; }

        string UpdateUrl { get; set; }

        string Passthrough { get; set; }
    }
}