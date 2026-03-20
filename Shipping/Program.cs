using Microsoft.EntityFrameworkCore;
using Shipping.Models;
using Shipping.Services;

namespace Shipping;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.AddServiceDefaults();
        builder.AddRabbitMQClient("messaging");
        builder.Services.AddHostedService<Worker>();
		builder.Services.AddDbContext<ShippingContext>(opt =>
			opt.UseInMemoryDatabase("OrderList")
		);

		var host = builder.Build();
        host.Run();
    }
}
