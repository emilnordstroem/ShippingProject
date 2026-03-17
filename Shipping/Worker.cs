using System.Text;
using System.Threading.Channels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Shipping;

public class Worker : IHostedService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConnection _connection;
    private IChannel _channel;

    public Worker(ILogger<Worker> logger, IConnection connection)
    {
        _logger = logger;
        _connection = connection;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker started.");

        _channel = await _connection.CreateChannelAsync();

		await _channel.QueueDeclareAsync(
		   queue: "orders",
		   durable: true,
		   exclusive: false,
		   autoDelete: false,
		   arguments: null
		);

		var consumer = new AsyncEventingBasicConsumer(_channel);
		consumer.ReceivedAsync += async (model, ea) =>
		{
			var body = ea.Body.ToArray();
			var message = Encoding.UTF8.GetString(body);
			_logger.LogInformation("Received order: {Message}", message);

			await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
		};

		await _channel.BasicConsumeAsync(
			queue: "orders",
			autoAck: false,
			consumer: consumer
		);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker stopped.");

		if (_channel is not null)
		{
			await _channel.CloseAsync();
			_channel.Dispose();
		}
    }
}
