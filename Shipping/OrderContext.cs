using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShippingModels;

namespace Shipping
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
