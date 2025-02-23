using Clever.Onboarding.CampaignDataAPI.Models;

namespace Clever.Onboarding.CampaignDataAPI.Repositories;
public class CampaignRepository : ICampaignRepository
{
    private readonly List<CampaignData> _campaigns = new List<CampaignData>
        {
            new CampaignData
            {
                Code = "1900000",
                Name = "Clever All-in-One EV",
                Product = "All-in-one installation",
                StartDate = new DateTime(2025, 1, 1),
                EndDate = new DateTime(2025, 3, 31),
                StartUpPrice = 7500
            },
            new CampaignData
            {
                Code = "1910772",
                Name = "Clever One Business EV",
                Product = "Complete EV",
                StartDate = new DateTime(2025, 1, 1),
                EndDate = new DateTime(2025, 4, 30)
            },
            new CampaignData
            {
                Code = "1920177",
                Name = "Clever One EV - Krogsgaard - 12 mdr. fri strøm - gratis installation - brugtbil",
                Product = "Unlimited EV",
                StartDate = new DateTime(2025, 6, 1),
                EndDate = new DateTime(2025, 8, 31),
                StartUpPrice = 7000
            }
        };

    public CampaignData GetCampaignByCode(string code)
    {
        return _campaigns.FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<CampaignData> GetAllCampaigns()
    {
        return _campaigns;
    }
}

