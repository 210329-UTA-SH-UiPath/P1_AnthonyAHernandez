using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzaBox.Domain.Entities;

namespace PizzaBox.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
   
        private readonly ILogger<OrdersController> _logger;
        private readonly PizzaBoxDbContext _context;
        public OrdersController(ILogger<OrdersController> logger, PizzaBoxDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Order> Get()
        {

            IEnumerable<Order> IEnumerableCustomerOrders = _context.Orders.Include(o => o.Customer).
                Include(o => o.Store)
                .Include(o => o.Pizzas).
                ThenInclude(p => p.PizzaToppings).ThenInclude(pt => pt.Topping)
                .Include(o => o.Pizzas).ThenInclude(p => p.Crust)
                .Include(o => o.Pizzas).ThenInclude(p => p.Size)
                .Where(o => o.OrderStatus == "Completed").ToList();

            return IEnumerableCustomerOrders;
        }
    }
}
