using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.model
{
    public class Product
    {
        public TypeProduct typeOfProduct { get; set; }
        public String nameOfProfuct { get; set; }
        public String descriptionOfProfuct { get; set; }
        public Double costOfProduct { get; set; }
        public int numberForPurchase { get; set; }
    }
}
