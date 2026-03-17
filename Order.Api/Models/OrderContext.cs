using Microsoft.EntityFrameworkCore;

namespace Order.Api.Models
{
	public class OrderContext : DbContext
	{
		public OrderContext(DbContextOptions<OrderContext> options)
			: base(options)
		{
		}
		public DbSet<OrderModel> Orders { get; set; } = default!;
	}
}
