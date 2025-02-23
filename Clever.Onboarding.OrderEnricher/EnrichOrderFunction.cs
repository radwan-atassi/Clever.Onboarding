using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Clever.Onboarding.OrderEnricher.Models;
using Clever.Onboarding.OrderEnricher.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Clever.Onboarding.OrderEnricher
{
    public class EnrichOrderFunction(ILogger<EnrichOrderFunction> logger, IOrderEnrichmentService orderEnrichmentService)
    {
        private readonly ILogger<EnrichOrderFunction> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IOrderEnrichmentService _orderEnrichmentService = orderEnrichmentService ?? throw new ArgumentNullException(nameof(orderEnrichmentService));

        [Function("order-enricher")]
        public async Task Run(
            [ServiceBusTrigger("raw-orders", Connection = "ServiceBusConnectionString")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation("Started handling raw order message: {message}", message.Body);
            try
            {
                var rawOrder = JsonSerializer.Deserialize<Order>(message.Body);

                if (rawOrder == null)
                {
                    _logger.LogWarning("Invalid raw order. Dead-lettering message.");
                    await messageActions.DeadLetterMessageAsync(message, deadLetterReason: "Something went wrong while handling service bus message.").ConfigureAwait(false);
                    return;
                }

                //TODO:Validate order before Enriching.

                await _orderEnrichmentService.EnrichAndPublishOrderAsync(rawOrder).ConfigureAwait(false);
                _logger.LogInformation("Successfully enriched order: {OrderID}. Pushing enriched order to service bus", rawOrder.OrderId);
                await messageActions.CompleteMessageAsync(message).ConfigureAwait(false);
                _logger.LogInformation("Successfully handled service bus message.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling raw order message.");
                throw;
            }
        }
    }
}
