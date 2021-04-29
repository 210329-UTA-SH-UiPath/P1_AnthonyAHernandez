using System.Collections.Generic;
using PizzaBox.Domain.Entities;
using PizzaBox.Domain.Models;

namespace PizzaBox.Client.Singletons
{
	public class PizzaSingleton
  {
    private static PizzaSingleton _instance;

    public List<Pizza> Pizzas { get; set; }
    public static PizzaSingleton Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = new PizzaSingleton();
        }

        return _instance;
      }
    }
    private PizzaSingleton()
    {
      Pizzas = new List<Pizza>()
      {
        new MeatPizza(),
        new VeganPizza(),
        new KetoFriendlyPizza(),
        new BbqChickenPizza(),
        new CreateYourOwn()
    };
    }
  }
}
