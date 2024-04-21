using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.model
{
    public class Product
    {
        public TypeProduct typeProduct { get; set; }
        public String nameProfuct { get; set; }
        public String descriptionProfuct { get; set; }
        public Double costProduct { get; set; }
        public int numberForPurchase { get; set; }

        public Product(TypeProduct typeProduct, string nameProfuct, string descriptionProfuct, double costProduct, int numberForPurchase)
        {
            this.typeProduct = typeProduct;
            this.nameProfuct = nameProfuct;
            this.descriptionProfuct = descriptionProfuct;
            this.costProduct = costProduct;
            this.numberForPurchase = numberForPurchase;
        }
    }
}
