namespace Clever.Onboarding.Order.Data.Models;

public class EnrichedOrder
{
    public int Id { get; set; } // Primary key for EF Core
    public string OrderId { get; set; }
    public string CustomerName { get; set; }
    public string CampaignCode { get; set; }
    public decimal? StartUpPrice { get; set; }
}
