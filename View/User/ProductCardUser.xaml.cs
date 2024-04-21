using Kurs.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kurs
{
    /// <summary>
    /// Interaction logic for ProductCardUser.xaml
    /// </summary>
    public partial class ProductCardUser : Window
    {
        private User user;
        private Product product;
        public ProductCardUser(User user, Product product)
        {
            InitializeComponent();
            this.user = user;
            this.product = product;

            typeProduct.Content = product.typeProduct;
            nameProduct.Content = product.nameProfuct;
            descriptionProduct.Content = product.descriptionProfuct;
            costProduct.Content = product.costProduct;
            numberForPurchase.Content = product.numberForPurchase;
        }
    }
}
