using Clever.Onboarding.CampaignDataAPI.Models;
using Clever.Onboarding.CampaignDataAPI.Repositories;

namespace Clever.Onboarding.CampaignDataAPI.Services;
public class CampaignService(ICampaignRepository repository) : ICampaignService
{
    private readonly ICampaignRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public CampaignData GetCampaignByCode(string code)
    {
        return _repository.GetCampaignByCode(code);
    }
}
