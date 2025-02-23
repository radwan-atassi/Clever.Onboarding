using Clever.Onboarding.OrderEnricher.Models;

namespace Clever.Onboarding.OrderEnricher.CampaignApiClient;
public interface ICampaignApiClient
{
    Task<CampaignData?> GetCampaignAsync(string campaignCode);
}
