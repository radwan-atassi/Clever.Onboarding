using Clever.Onboarding.OrderProcessor.Models;

namespace Clever.Onboarding.OrderProcessor.Services;

public interface IOrderPublisher
{
    Task PublishRawOrderAsync(Order order);
}

