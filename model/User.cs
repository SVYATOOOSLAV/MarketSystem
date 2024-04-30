using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.model
{
    public class User
    {
        public String login { get; set; }
        public Double budget { get; set; }
        public Dictionary<Product, int> basket = new Dictionary<Product, int>();
        
        public User(String login, Double budget)
        {
            this.login = login;
            this.budget = budget;
        }

        public void addProductToBasket(Product product)
        {
            if (basket.ContainsKey(product))
            {
                basket[product] += 1;
            }
            else
            {
                basket.Add(product, 1);
            }
        }
    }
}
