using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using ShippingModels;

namespace Order.Api.Models
{
	public class OutboxWorker : BackgroundService
	{
	    private readonly IConnection _connection;
		private readonly IServiceScopeFactory _scopeFactory;

		public OutboxWorker(IConnection connection, IServiceScopeFactory scopeFactory)
		{
			_connection = connection;
			_scopeFactory = scopeFactory;
		}
		protected async override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				await ProcessOutboxMessage();
				await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
			}
		}

		private async Task ProcessOutboxMessage()
		{
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<OrderContext>();

			await using IChannel channel = await _connection.CreateChannelAsync();

			await channel.QueueDeclareAsync(
				queue: "shipping_queue",
				durable: true,
				exclusive: false,
				autoDelete: false,
				arguments: null
			);

			List<OutboxMessage> unsentMessages = await context.Outbox
				.Where(message => message.ProcessedAtUTC == null)
				.ToListAsync();

			foreach (OutboxMessage message in unsentMessages)
			{
				var body = Encoding.UTF8.GetBytes(message.Payload);

				await channel.BasicPublishAsync(
					exchange: string.Empty,
					routingKey: "shipping_queue",
					body: body
				);

				message.ProcessedAtUTC = DateTime.UtcNow;
			}

			await context.SaveChangesAsync();
		}
	}
}