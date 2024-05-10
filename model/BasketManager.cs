using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.model
{
    public class BasketManager
    {
        private Dictionary<Product, int> basket = new Dictionary<Product, int>();

        public Dictionary<Product, int> getBasket() => basket;

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

        public void removeProductFromBasket(Product product)
        {
            if (basket.ContainsKey(product) && basket[product] > 1)
            {
                basket[product] -= 1;
            }
            else
            {
                basket.Remove(product);
            }
        }
    }
}
