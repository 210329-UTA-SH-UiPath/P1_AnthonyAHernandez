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
    public class CustomerController : ControllerBase
    {

        private readonly ILogger<CustomerController> _logger;
        private readonly PizzaBoxDbContext _context;
        public CustomerController(ILogger<CustomerController> logger, PizzaBoxDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            var customers = _context.Customers.ToList();
            return customers;
        }

        [HttpPost]

        public Customer Post(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return customer;
        }
    }
}
