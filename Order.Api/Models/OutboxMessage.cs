namespace Order.Api.Models
{
	public class OutboxMessage
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public required string Type { get; set; }
		public required string Payload { get; set; }
		public DateTime? ProcessedAtUTC { get; set; }

		public OutboxMessage ()
		{

		}
	}
}