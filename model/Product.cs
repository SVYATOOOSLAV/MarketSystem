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
        public String nameProduct { get; set; }
        public String descriptionProduct { get; set; }
        public Double costProduct { get; set; }
        public int numberForPurchase { get; set; }

        public Product(TypeProduct typeProduct, string nameProfuct, string descriptionProfuct, double costProduct, int numberForPurchase)
        {
            this.typeProduct = typeProduct;
            this.nameProduct = nameProfuct;
            this.descriptionProduct = descriptionProfuct;
            this.costProduct = costProduct;
            this.numberForPurchase = numberForPurchase;
        }

        public override bool Equals(object obj)
        {
            return obj is Product product &&
                   typeProduct == product.typeProduct &&
                   nameProduct == product.nameProduct &&
                   descriptionProduct == product.descriptionProduct &&
                   costProduct == product.costProduct;
        }

        public override int GetHashCode()
        {
            int hashCode = -61797700;
            hashCode = hashCode * -1521134295 + typeProduct.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(nameProduct);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(descriptionProduct);
            hashCode = hashCode * -1521134295 + costProduct.GetHashCode();
            return hashCode;
        }
    }
}
