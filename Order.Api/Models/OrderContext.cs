using Microsoft.EntityFrameworkCore;
using ShippingModels;

namespace Order.Api.Models
{
	public class OrderContext : DbContext
	{
		public OrderContext(DbContextOptions<OrderContext> options)
			: base(options)
		{
		}
		public DbSet<OrderModel> Orders { get; set; } = default!;
		public DbSet<OutboxMessage> Outbox { get; set; } = default!;
	}
}
