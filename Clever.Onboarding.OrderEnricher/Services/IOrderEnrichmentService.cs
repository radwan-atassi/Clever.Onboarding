using Clever.Onboarding.OrderEnricher.Models;

namespace Clever.Onboarding.OrderEnricher.Services;

public interface IOrderEnrichmentService
{
    Task EnrichAndPublishOrderAsync(Order rawOrder);
}

