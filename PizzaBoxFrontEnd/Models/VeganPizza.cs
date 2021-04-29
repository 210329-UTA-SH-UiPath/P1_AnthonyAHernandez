using PizzaBox.Domain.Entities;

namespace PizzaBox.Domain.Models
{
	public class VeganPizza : Pizza
	{

		public VeganPizza()
		{
			Name = "Grass Consumer Pizza";
			var spinachTopping = PizzaHelper.GetTopping("Spinach");
			if (spinachTopping != null)
            {
				PizzaToppings.Add(new PizzaTopping { ToppingId = spinachTopping.Id, Topping = spinachTopping });
			}
			

			var olivesTopping = PizzaHelper.GetTopping("Olives");
			if (olivesTopping != null)
            {
				PizzaToppings.Add(new PizzaTopping { ToppingId = olivesTopping.Id, Topping = olivesTopping });
				CrustId = 1;
				SizeId = 1;
			}

		}
	}
}
