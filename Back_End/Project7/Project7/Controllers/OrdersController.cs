using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project7.DTOs;
using Project7.Models;

namespace Project7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly MyDbContext _context;

        public OrdersController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] CreateOrderDTO newOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = new Order
            {
                UserId = newOrder.UserId,
                TotalAmount = newOrder.TotalAmount,
                PaymentMethod = newOrder.PaymentMethod,
                OrderStatus = newOrder.OrderStatus,
                Comment = newOrder.Comment,
                OrderDate = newOrder.OrderDate
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            return Ok(order);
        }



    }
}
