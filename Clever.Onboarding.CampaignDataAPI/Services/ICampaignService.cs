using Clever.Onboarding.CampaignDataAPI.Models;

namespace Clever.Onboarding.CampaignDataAPI.Services;
public interface ICampaignService
{
    CampaignData GetCampaignByCode(string code);
}

