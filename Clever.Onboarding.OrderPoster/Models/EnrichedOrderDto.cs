namespace Clever.Onboarding.OrderPoster.Models;

public class EnrichedOrderDto
{
    public string OrderId { get; set; }
    public string CustomerName { get; set; }
    public string CampaignCode { get; set; }
    public decimal? StartUpPrice { get; set; }
}
