using Microsoft.EntityFrameworkCore;
using Shipping.API.Models;
using Shipping.API.Services;
namespace Shipping.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

		// Add services to the container.
		builder.AddRabbitMQClient("messaging");
		builder.Services.AddHostedService<Worker>();
		builder.Services.AddDbContext<ShippingContext>(opt =>
			opt.UseInMemoryDatabase("OrderList")
		);

		builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
