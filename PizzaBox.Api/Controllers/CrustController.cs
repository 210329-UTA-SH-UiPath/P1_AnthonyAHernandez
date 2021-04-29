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
    public class CrustController : ControllerBase
    {

        private readonly ILogger<CrustController> _logger;
        private readonly PizzaBoxDbContext _context;
        public CrustController(ILogger<CrustController> logger, PizzaBoxDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Crust> Get()
        {
            var crusts = _context.Crusts.ToList();
            return crusts;
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
