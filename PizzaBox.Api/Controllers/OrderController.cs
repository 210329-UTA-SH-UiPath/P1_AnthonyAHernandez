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
    public class OrderController : ControllerBase
    {

        private readonly ILogger<CustomerController> _logger;
        private readonly PizzaBoxDbContext _context;
        public OrderController(ILogger<CustomerController> logger, PizzaBoxDbContext context)
        {
            _logger = logger;
            _context = context;
        }
	
		[HttpGet]

		public Order Get(int id)
        {
			return _context.Orders
			.Include(o => o.Customer).
				Include(o => o.Store)
				.Include(o => o.Pizzas).
				ThenInclude(p => p.PizzaToppings).ThenInclude(pt => pt.Topping)
				.Include(o => o.Pizzas).ThenInclude(p => p.Crust)
				.Include(o => o.Pizzas).ThenInclude(p => p.Size)
				.Where(o => o.Id == id).FirstOrDefault();
		}
		[HttpPost]

        public Order Post(Order order)
        {
			_context.Orders.Add(order);
			_context.SaveChanges();
			return order;
		}
		[HttpPut]

		public Order Put(Order order)
		{
			_context.Update(order);
			_context.SaveChanges();
			return order;
		}
	}
}
