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
    public class SizeController : ControllerBase
    {

        private readonly ILogger<SizeController> _logger;
        private readonly PizzaBoxDbContext _context;
        public SizeController(ILogger<SizeController> logger, PizzaBoxDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Size> Get()
        {
            var sizes = _context.Sizes.ToList();
            return sizes;
        
        }
    }
}
