using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Clever.Onboarding.Order.Data.Models;
using Clever.Onboarding.Order.Data.Repositories;
using Clever.Onboarding.OrderPersister.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Clever.Onboarding.OrderPersister;

public class OrderPersistenceFunction(ILogger<OrderPersistenceFunction> logger, IOrderRepository orderRepository)
{
    private readonly ILogger<OrderPersistenceFunction> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));

    [Function("order-persistence-function")]
    public async Task Run(
        [ServiceBusTrigger("enriched-orders-topic", "order-persistence-subscription", Connection = "ServiceBusConnectionString")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Started handling incoming Service Bus message. Message ID: {MessageId}", message.MessageId);

        try
        {
            var enrichedOrder = JsonSerializer.Deserialize<EnrichedOrderDto>(message.Body);

            if (enrichedOrder == null)
            {
                _logger.LogError("Could not parse enriched order from Service Bus message: {MessageId}", message.MessageId);
                await messageActions.DeadLetterMessageAsync(message, deadLetterReason: "Error parsing EnrichedOrder from Service bus message").ConfigureAwait(false);
            }

            var order = new EnrichedOrder
            {
                OrderId = enrichedOrder.OrderId,
                CustomerName = enrichedOrder.CustomerName,
                CampaignCode = enrichedOrder.CampaignCode,
                StartUpPrice = enrichedOrder.StartUpPrice
            };
            
            await _orderRepository.AddEnrichedOrderAsync(order).ConfigureAwait(false);
            _logger.LogInformation("Order persisted successfully with OrderId: {OrderId}", order.OrderId);
            await messageActions.CompleteMessageAsync(message).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong while saving order.");
            throw;
        }
    }
}

