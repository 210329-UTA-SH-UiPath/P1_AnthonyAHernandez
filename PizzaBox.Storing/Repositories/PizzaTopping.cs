using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaBox.Domain.Entities
{
	public class PizzaTopping
	{
		public int Id { get; set; }

		public int PizzaId { get; set; }

		public int ToppingId { get; set; }
		public virtual Topping Topping { get; set; }

	}
}
