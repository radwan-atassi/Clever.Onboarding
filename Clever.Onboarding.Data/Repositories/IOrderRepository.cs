using Clever.Onboarding.Order.Data.Models;

namespace Clever.Onboarding.Order.Data.Repositories;

public interface IOrderRepository
{
    Task AddEnrichedOrderAsync(EnrichedOrder order);
}

