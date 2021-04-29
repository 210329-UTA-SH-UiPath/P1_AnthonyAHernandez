using System;
using PizzaBox.Domain.Entities;

namespace PizzaBoxFrontEnd.Models
{
    public class AddPizzaViewModel
    {
        public int PizzaIndex{ get; set; }
        public Pizza Pizza { get; set; }
        public int OrderId { get; set; }
    }
}
