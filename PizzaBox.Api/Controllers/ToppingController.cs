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
    public class ToppingController : ControllerBase
    {

        private readonly ILogger<CustomerController> _logger;
        private readonly PizzaBoxDbContext _context;
        public ToppingController(ILogger<CustomerController> logger, PizzaBoxDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public List<Topping> Get()
        {
            return _context.Toppings.ToList();

        }

        }
    }

