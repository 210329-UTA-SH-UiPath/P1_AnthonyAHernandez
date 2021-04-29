using System;
using System.Collections.Generic;

#nullable disable

namespace PizzaBox.Domain.Entities
{
	public partial class Pizza
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CrustId { get; set; }
		public int SizeId { get; set; }
		public int Quantity { get; set; }
		public int OrderId { get; set; }

		public virtual Crust Crust { get; set; }
		public virtual Size Size { get; set; }
		public virtual List<PizzaTopping> PizzaToppings { get; set; }

		public Pizza()
		{
			PizzaToppings = new List<PizzaTopping>();
		}

		public decimal GetCost()
		{
			decimal pizzaPrice = 0m;
			pizzaPrice += Size.Price;
			pizzaPrice += Crust.Price;
			foreach (var topping in PizzaToppings)
			{
				pizzaPrice += topping.Topping.Price;
			}
			return pizzaPrice * Quantity;
		}
	}
}
