using Clever.Onboarding.OrderPoster.CRMApiClient;
using Clever.Onboarding.OrderPoster.Models;

namespace Clever.Onboarding.OrderPoster.CrmApiClient;

internal class CrmApiClient : ICrmApiClient
{
    public Task<HttpResponseMessage> TryPostOrderAsync(EnrichedOrderDto enrichedOrder)
    {
        throw new NotImplementedException();
    }
}

