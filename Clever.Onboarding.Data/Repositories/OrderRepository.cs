using Clever.Onboarding.Order.Data.Data;
using Clever.Onboarding.Order.Data.Models;

namespace Clever.Onboarding.Order.Data.Repositories;

public class OrderRepository(OrderDbContext context) : IOrderRepository
{
    public async Task AddEnrichedOrderAsync(EnrichedOrder order)
    {
        context.EnrichedOrders.Add(order);
        await context.SaveChangesAsync().ConfigureAwait(false);
    }
}

