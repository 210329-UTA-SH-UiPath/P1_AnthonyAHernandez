using System.Linq;
using PizzaBox.Domain.Models;
using Xunit;

namespace PizzaBox.Testing.Tests
{
  /// <summary>
  /// 
  /// </summary>
  public class PizzaTests
  {
    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void MeatPizza_ShouldHaveCorrectName()
    {
      // arrange
      var sut = new MeatPizza();

      // act
      var actual = sut.Name;

      // assert
      Assert.Equal("Meat Pizza", actual);
    }
        [Fact]
        public void VeganPizza_ShouldHaveCorrectName()
        {
            // arrange
            var sut = new VeganPizza();

            // act
            var actual = sut.Name;

            // assert
            Assert.Equal("Vegan Pizza", actual);
        }
        [Fact]
        public void BbqChickenPizza_ShouldHaveCorrectName()
        {
            // arrange
            var sut = new BbqChickenPizza();

            // act
            var actual = sut.Name;

            // assert
            Assert.Equal("Bbq Chicken Pizza", actual);
        }
        [Fact]
        public void KetoFriendlyPizza_ShouldHaveCorrectName()
        {
            // arrange
            var sut = new KetoFriendlyPizza();

            // act
            var actual = sut.Name;

            // assert
            Assert.Equal("Keto Friendly Pizza", actual);
        }
        [Fact]
        public void CreateYourOwn_ShouldHaveCorrectName()
        {
            // arrange
            var sut = new CreateYourOwn();

            // act
            var actual = sut.Name;

            // assert
            Assert.Equal("Create Your Own", actual);
        }

        [Fact]
        public void CreateYourOwn_ShouldHaveNoDefaultToppings()
        {
            // arrange
            var sut = new CreateYourOwn();

            // act
            var actual = sut.PizzaToppings;

            // assert
            Assert.True(!actual.Any()); //when it is true actually does not have any toppings
        }

        [Fact]
        public void MeatPizza_ShouldHaveCorrectDefaultToppings()
        {
            // arrange
            var sut = new MeatPizza();

            // act
            var actual = sut.PizzaToppings;

            // assert
            Assert.Equal(2, actual.Count()); //when it is true it has 2 toppings
            var firstTopping = actual[0];
            Assert.Equal("Bacon", firstTopping.Topping.Name);
            var secondTopping = actual[1];
            Assert.Equal("Sausage", secondTopping.Topping.Name);
        }

        [Fact]
        public void BbqChickenPizza_ShouldHaveCorrectDefaultToppings()
        {
            // arrange
            var sut = new BbqChickenPizza();

            // act
            var actual = sut.PizzaToppings;

            // assert
            Assert.Equal(2, actual.Count()); //when it is true it has 2 toppings
            var firstTopping = actual[0];
            Assert.Equal("Chicken", firstTopping.Topping.Name);
            var secondTopping = actual[1];
            Assert.Equal("BBQ Sauce", secondTopping.Topping.Name);
        }
        [Fact]
        public void KetoFriendlyPizza_ShouldHaveCorrectDefaultToppings()
        {
            // arrange
            var sut = new KetoFriendlyPizza();

            // act
            var actual = sut.PizzaToppings;

            // assert
            Assert.Equal(2, actual.Count()); //when it is true it has 2 toppings
            var firstTopping = actual[0];
            Assert.Equal("Extra Cheese", firstTopping.Topping.Name);
            var secondTopping = actual[1];
            Assert.Equal("Bacon", secondTopping.Topping.Name);
        }
        [Fact]
        public void Vegan_ShouldHaveCorrectDefaultToppings()
        {
            // arrange
            var sut = new VeganPizza();

            // act
            var actual = sut.PizzaToppings;

            // assert
            Assert.Equal(2, actual.Count()); //when it is true it has 2 toppings
            var firstTopping = actual[0];
            Assert.Equal("Spinach", firstTopping.Topping.Name);
            var secondTopping = actual[1];
            Assert.Equal("Olives", secondTopping.Topping.Name);
        }

    }
}
