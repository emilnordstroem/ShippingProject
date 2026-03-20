using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShippingModels;

namespace Shipping.Models
{
	public class ShippingContext : DbContext
	{
		public ShippingContext(DbContextOptions<ShippingContext> options)
			: base(options)
		{
		}
		public DbSet<ShippingOrder> ShippingOrders { get; set; } = default!;
	}
}
