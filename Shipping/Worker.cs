using RabbitMQ.Client;

namespace Shipping;

public class Worker : IHostedService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConnection _connection;

    public Worker(ILogger<Worker> logger, IConnection connection)
    {
        _logger = logger;
        _connection = connection;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker started.");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker stopped.");
        return Task.CompletedTask;
    }
}
