using System;
using System.Collections.Generic;

#nullable disable

namespace PizzaBox.Domain.Entities
{
    public partial class Size
    {
        public Size()
        {

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

    }
}
