using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.Api.Models;

namespace Order.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderModelsController : ControllerBase
    {
        private readonly OrderContext _context;

        public OrderModelsController(OrderContext context)
        {
            _context = context;
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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderModel(int id, OrderModel orderModel)
        {
            if (id != orderModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(orderModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<OrderModel>> PostOrderModel(OrderModel orderModel)
        {
            _context.Orders.Add(orderModel);
            await _context.SaveChangesAsync();

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

        private bool OrderModelExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
