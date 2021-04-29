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
    public class PizzaToppingController : ControllerBase
    {

        private readonly ILogger<PizzaToppingController> _logger;
        private readonly PizzaBoxDbContext _context;
        public PizzaToppingController(ILogger<PizzaToppingController> logger, PizzaBoxDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public PizzaTopping Post(PizzaTopping pizzaTopping)
        {
            pizzaTopping.Id = 0;
            pizzaTopping.Topping = null;
            _context.PizzaToppings.Add(pizzaTopping);
            _context.SaveChanges();

            return pizzaTopping;

        }

        [HttpPut]
        public PizzaTopping Put(PizzaTopping pizzaTopping)
        {
            _context.PizzaToppings.Update(pizzaTopping);
            _context.SaveChanges();

            return pizzaTopping;

        }

        [HttpDelete]
        public PizzaTopping Delete(int id)
        {
            var pizzaTopping = _context.PizzaToppings.Where(i => i.Id == id).FirstOrDefault();
            if (pizzaTopping != null)
            {
                _context.PizzaToppings.Remove(pizzaTopping);
                _context.SaveChanges();

            }

            return pizzaTopping;

        }
    }
    }

