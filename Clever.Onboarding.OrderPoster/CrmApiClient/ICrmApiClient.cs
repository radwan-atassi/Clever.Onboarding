using Clever.Onboarding.OrderPoster.Models;

namespace Clever.Onboarding.OrderPoster.CRMApiClient;

public interface ICrmApiClient
{
    Task<HttpResponseMessage> TryPostOrderAsync(EnrichedOrderDto enrichedOrder);
}
