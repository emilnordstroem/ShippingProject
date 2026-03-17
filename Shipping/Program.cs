using Microsoft.EntityFrameworkCore;

namespace Shipping;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.AddServiceDefaults();
        builder.AddRabbitMQClient("messaging");
        builder.Services.AddHostedService<Worker>();
		builder.Services.AddDbContext<OrderContext>(opt =>
			opt.UseInMemoryDatabase("OrderList")
		);

		var host = builder.Build();
        host.Run();
    }
}
