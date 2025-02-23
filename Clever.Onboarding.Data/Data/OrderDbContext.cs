using Clever.Onboarding.Order.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Clever.Onboarding.Order.Data.Data;
public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
    public DbSet<EnrichedOrder> EnrichedOrders { get; set; }
}
