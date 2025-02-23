using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Clever.Onboarding.OrderProcessor.Models;

namespace Clever.Onboarding.OrderProcessor.Services;

public class OrderPublisher(ServiceBusClient serviceBusClient, string queueName) : IOrderPublisher
{
    private readonly ServiceBusClient _serviceBusClient = serviceBusClient ?? throw new ArgumentNullException(nameof(serviceBusClient));
    private readonly string _queueName = queueName ?? throw new ArgumentNullException(nameof(queueName));

    public async Task PublishRawOrderAsync(Order order)
    {
        ServiceBusSender sender = _serviceBusClient.CreateSender(_queueName);
        var messageBody = JsonSerializer.Serialize(order);
        ServiceBusMessage message = new ServiceBusMessage(messageBody);
        await sender.SendMessageAsync(message).ConfigureAwait(false);
    }
}
