namespace Order.Api.Models
{
	public class OrderModel
	{
		public int Id { get; set; }
		public string CustomerName { get; set; }
		public DateTime PlacementDate { get; set; }
		public string[] Items { get; set; }
		public double Amount { get; set; }

		public OrderModel() 
		{ 
		}
	}
}
