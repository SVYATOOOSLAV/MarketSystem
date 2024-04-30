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

namespace Kurs.View.user
{
    /// <summary>
    /// Interaction logic for Basket.xaml
    /// </summary>
    public partial class BasketWindow : Window
    {
        private User user;
        private List<DesiredProduct> availableProducts;

        public BasketWindow(User user)
        {
            InitializeComponent();
            this.user = user;

            userLabel.Content += user.login;

            showBasketUser();
        }

        private void showBasketUser()
        {
            availableProducts = user.basket.Select(product => 
                new DesiredProduct(
                    product.Key.typeProduct,
                    product.Key.nameProduct,
                    product.Key.descriptionProfuct,
                    product.Key.costProduct,
                    product.Key.numberForPurchase,
                    product.Value)
                ).ToList();

            mainDataGrid.ItemsSource = availableProducts;
        }
    }
}
