using System.Net;
using System.Net.Http.Json;
using Clever.Onboarding.OrderEnricher.Models;
using Microsoft.Extensions.Logging;

namespace Clever.Onboarding.OrderEnricher.CampaignApiClient
{
    public class CampaignApiClient(HttpClient httpClient, ILogger<CampaignApiClient> logger) : ICampaignApiClient
    {
        private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        private readonly ILogger<CampaignApiClient> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<CampaignData?> GetCampaignAsync(string campaignCode)
        {
            string url = $"http://localhost:63119/api/Campaigns/{campaignCode}";

            var response = await _httpClient.GetAsync(url).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CampaignData>().ConfigureAwait(false);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Campaign code {code} not found (404).", campaignCode);
                return null;
            }
            else
            {
                string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string msg = $"Error from Campaign API (status: {response.StatusCode}): {body}";

                _logger.LogError("Campaign API returned error: {Status} {Body}", response.StatusCode, body);
                throw new HttpRequestException(msg);
            }
        }
    }
}