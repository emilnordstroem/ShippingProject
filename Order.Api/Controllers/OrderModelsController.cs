using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.Api.Models;
using RabbitMQ.Client;
using ShippingModels;

namespace Order.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderModelsController : ControllerBase
    {
        private readonly OrderContext _context;
        private readonly IConnection _connection;

        public OrderModelsController(OrderContext context, IConnection connection)
        {
            _context = context;
            _connection = connection;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderModel>> GetOrderModel(int id)
        {
            var orderModel = await _context.Orders.FindAsync(id);

            if (orderModel == null)
            {
                return NotFound();
            }

            return orderModel;
        }

        [HttpPost]
        public async Task<ActionResult<OrderModel>> PostOrderModel(OrderModel orderModel)
        {
            _context.Orders.Add(orderModel);
            await _context.SaveChangesAsync();

			await using IChannel channel = await _connection.CreateChannelAsync();

			await channel.QueueDeclareAsync(
				queue: "shipping_queue",
				durable: true,
				exclusive: false,
				autoDelete: false,
				arguments: null
			);

			var message = JsonSerializer.Serialize(orderModel);
			var body = Encoding.UTF8.GetBytes(message);

			await channel.BasicPublishAsync(
				exchange: string.Empty,
				routingKey: "shipping_queue",
				body: body
			);


			return CreatedAtAction("GetOrderModel", new { id = orderModel.Id }, orderModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderModel(int id)
        {
            var orderModel = await _context.Orders.FindAsync(id);
            if (orderModel == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(orderModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}