namespace Clever.Onboarding.OrderEnricher.Models;
public class CampaignData
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Product { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal? StartUpPrice { get; set; }
}

