namespace Clever.Onboarding.OrderProcessor.Models;

public class Order
{
    public string OrderId { get; set; }
    public string CustomerName { get; set; }
    public string CampaignCode { get; set; }
    public string CustomerAddress { get; set; }
    public string ProductName { get; set; }
}