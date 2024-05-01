using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.model
{
    public class Order
    {
        public DateTime time {  get; set; }
        public String login {  get; set; }
        public String nameProduct {  get; set; }
        public int countPurchase {  get; set; }

        public Order(DateTime time, String login, String nameProduct, int countPurchase)
        {
            this.time = time;
            this.login = login; 
            this.nameProduct = nameProduct;
            this.countPurchase = countPurchase;
        }
    }
}
