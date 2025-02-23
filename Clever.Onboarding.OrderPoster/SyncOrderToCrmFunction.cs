using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Clever.Onboarding.OrderPoster.CRMApiClient;
using Clever.Onboarding.OrderPoster.Models;

namespace Clever.Onboarding.OrderPoster;
public class SyncOrderToCrmFunction(ILogger<SyncOrderToCrmFunction> logger, ICrmApiClient crmApiClient)
{
    private readonly ILogger<SyncOrderToCrmFunction> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly ICrmApiClient _crmApiClient = crmApiClient ?? throw new ArgumentNullException(nameof(crmApiClient));

    [Function("sync-order-to-crm-function")]
    public async Task Run(
         [ServiceBusTrigger("enriched-orders-topic", "order-api-subscription", Connection = "ServiceBusConnectionString")]
            ServiceBusReceivedMessage message,
         ServiceBusMessageActions messageActions)
    {
        {
            _logger.LogInformation("Received enriched order message: {message}", message);
            var enrichedOrder = JsonSerializer.Deserialize<EnrichedOrderDto>(message.Body);
            
            if (enrichedOrder == null)
            {
                _logger.LogError("Failed to deserialize message: {message}", message.Body);
                return;
            }

            try
            {
                var response = await _crmApiClient.TryPostOrderAsync(enrichedOrder).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully forwarded order {OrderId} to external Order API", enrichedOrder.OrderId);
                    await messageActions.CompleteMessageAsync(message).ConfigureAwait(false);
                }
                else
                {
                    _logger.LogError("Failed to forward order {OrderId} to external Order API. StatusCode: {StatusCode}", enrichedOrder.OrderId, response.StatusCode);
                    await messageActions.DeadLetterMessageAsync(message, deadLetterReason: "Error syncing order to CRM").ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while calling the external Order API for OrderId: {OrderId}", enrichedOrder.OrderId);
                throw;
            }
        }
    }
}
