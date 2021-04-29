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

		//private Order PlaceOrder(Order order)
		//{
			
		//	foreach (var pizza in order.Pizzas)
		//	{
		//		var newPizza = new Pizza
		//		{
		//			OrderId = newOrder.Id,
		//			Name = pizza.Name,
		//			SizeId = pizza.SizeId,
		//			CrustId = pizza.CrustId,
		//			Quantity = pizza.Quantity
		//		};
		//		_context.Pizzas.Add(newPizza);
		//		_context.SaveChanges();

		//		foreach (var pizzaTopping in pizza.PizzaToppings)
		//		{
		//			var newpizzaTopping = new PizzaTopping
		//			{
		//				PizzaId = newPizza.Id,
		//				ToppingId = pizzaTopping.ToppingId
		//			};
		//			_context.PizzaToppings.Add(newpizzaTopping);
		//			_context.SaveChanges();
		//		}
		//	}
		//	_context.SaveChanges();
		//	return newOrder;

		//}
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
