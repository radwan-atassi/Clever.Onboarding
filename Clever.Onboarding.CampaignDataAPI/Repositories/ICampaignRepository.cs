using Clever.Onboarding.CampaignDataAPI.Models;

namespace Clever.Onboarding.CampaignDataAPI.Repositories
{
    public interface ICampaignRepository
    {
        CampaignData GetCampaignByCode(string code);
        IEnumerable<CampaignData> GetAllCampaigns();
    }
}
