using PizzaBox.Domain.Entities;

namespace PizzaBox.Domain.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateYourOwn : Pizza
    {

        public CreateYourOwn()
        {
            Name = "Create Your Own";
            CrustId = 1;
            SizeId = 1;
        }
    }
}
