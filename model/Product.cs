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

        public override bool Equals(object obj)
        {
            return obj is Product product &&
                   typeProduct == product.typeProduct &&
                   nameProfuct == product.nameProfuct &&
                   descriptionProfuct == product.descriptionProfuct &&
                   costProduct == product.costProduct &&
                   numberForPurchase == product.numberForPurchase;
        }

        public override int GetHashCode()
        {
            int hashCode = -61797700;
            hashCode = hashCode * -1521134295 + typeProduct.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(nameProfuct);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(descriptionProfuct);
            hashCode = hashCode * -1521134295 + costProduct.GetHashCode();
            hashCode = hashCode * -1521134295 + numberForPurchase.GetHashCode();
            return hashCode;
        }
    }
}
