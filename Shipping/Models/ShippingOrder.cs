using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shipping.Models
{
	public class ShippingOrder
	{
		[Key]
		public Guid ShippingId { get; set; }
		public Guid? OrderId { get; set; }
		public string ShippingAdress { get; set; } = string.Empty;
		public ShippingStatus Status { get; set; } = ShippingStatus.Pending;

	}

	public enum ShippingStatus
	{
		Pending,
		Shipped,
		Delivered,
		Cancelled
	}
}
