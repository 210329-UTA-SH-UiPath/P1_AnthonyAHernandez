using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PizzaBox.Domain.Entities;

namespace PizzaBox.Domain
{
	public static class PizzaHelper
	{
		public static Topping GetTopping(string name)
		{
			try
            {
				var client = new HttpClient();
				string url = "http://localhost:5002/Topping";
				var response = client.GetAsync(url);

				if (response.Result.IsSuccessStatusCode)
				{
					var responsebody = response.Result.Content.ReadAsStringAsync().Result;
					var toppings = JsonConvert.DeserializeObject<List<Topping>>(responsebody);
					var topping = toppings.Where(t => t.Name == name).FirstOrDefault();
					return topping;
				}
				return null;

			}
			 catch
            {
				return null;
            }
			}
		

		
		
	}
}
