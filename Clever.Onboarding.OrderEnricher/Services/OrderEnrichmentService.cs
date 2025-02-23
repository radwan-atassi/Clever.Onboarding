using Azure.Messaging.ServiceBus;
using Clever.Onboarding.OrderEnricher.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Clever.Onboarding.OrderEnricher.CampaignApiClient;

namespace Clever.Onboarding.OrderEnricher.Services;

public class OrderEnrichmentService(
    ILogger<OrderEnrichmentService> logger,
    ServiceBusClient serviceBusClient,
    ICampaignApiClient campaignApiClient)
    : IOrderEnrichmentService
{
    private readonly ILogger<OrderEnrichmentService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly ServiceBusClient _serviceBusClient = serviceBusClient ?? throw new ArgumentNullException(nameof(serviceBusClient));

    private readonly string _enrichedTopicName = Environment.GetEnvironmentVariable("ServiceBusEnrichedTopicName") ?? throw new InvalidOperationException("Missing ServiceBusEnrichedTopicName env var.");

    private readonly ICampaignApiClient _campaignApiClient = campaignApiClient ?? throw new ArgumentNullException(nameof(campaignApiClient));

    public async Task EnrichAndPublishOrderAsync(Order rawOrder)
    {
        var enrichedOrder = new EnrichedOrder
        {
            OrderId = rawOrder.OrderId,
            CustomerName = rawOrder.CustomerName,
            CampaignCode = rawOrder.CampaignCode
        };

        if (!string.IsNullOrEmpty(rawOrder.CampaignCode))
        {
            var campaignData = await _campaignApiClient.GetCampaignAsync(rawOrder.CampaignCode).ConfigureAwait(false);

            if (campaignData != null)
            {
                enrichedOrder.StartUpPrice = campaignData.StartUpPrice;
            }
        }

        var enrichedOrderMessage = JsonSerializer.Serialize(enrichedOrder);
        ServiceBusSender sender = _serviceBusClient.CreateSender(_enrichedTopicName);
        await sender.SendMessageAsync(new ServiceBusMessage(enrichedOrderMessage)).ConfigureAwait(false);
        _logger.LogInformation("Enriched order published: {OrderId}", enrichedOrder.OrderId);
    }
}
