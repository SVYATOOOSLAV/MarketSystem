using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.model
{
    public class DesiredProduct : Product
    {
        public int desiredCount { get; set; }
        public DesiredProduct(TypeProduct typeProduct, string nameProfuct, string descriptionProfuct, double costProduct, int numberForPurchase, int desiredCount) :
            base(typeProduct, nameProfuct, descriptionProfuct, costProduct, numberForPurchase)
        {
            this.desiredCount = desiredCount;
        }
    }
}
