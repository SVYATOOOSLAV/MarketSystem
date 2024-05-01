using Kurs.model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
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
using Kurs.View.admin;

namespace Kurs.View.user
{
    /// <summary>
    /// Interaction logic for Basket.xaml
    /// </summary>
    public partial class BasketWindow : Window
    {
        private User user;
        private List<DesiredProduct> availableProducts;
        private double resultPrice;
        private DataBase dataBase = new DataBase();
        private List<Product> products;

        public BasketWindow(User user, List<Product> products)
        {
            InitializeComponent();
            this.user = user;

            userLabel.Content += user.login;

            updateDataOnWindow();

            this.products = products;
        }

        private void updateDataOnWindow()
        {
            showBasketUser();

            updateResultPrice();

            budgetUser.Text = user.budget + " руб.";
        }

        private void showBasketUser()
        {
            availableProducts = user.basket.Select(product =>
                new DesiredProduct(
                    product.Key.typeProduct,
                    product.Key.nameProduct,
                    product.Key.descriptionProduct,
                    product.Key.costProduct,
                    product.Key.numberForPurchase,
                    product.Value)
                ).ToList();

            mainDataGrid.ItemsSource = availableProducts;
        }

        private void updateResultPrice()
        {
            resultPrice = 0;
            foreach (var product in mainDataGrid.Items)
            {
                DesiredProduct row = product as DesiredProduct;
                if (row != null)
                {
                    resultPrice += row.costProduct * row.desiredCount;
                }
            }

            ResultPriceValue.Text = resultPrice + " руб.";
        }

        // Переход в личный кабинет
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserLC lc = new UserLC(user, this);
            lc.ShowDialog();
            budgetUser.Text = user.budget + " руб.";
        }

        private void PurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (user.budget < resultPrice)
            {
                MessageBoxResult result = MessageBox.Show("У вас недостаточно средств, поплните их", "Информирование", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                {
                    Button_Click(sender, e);
                }
            }

            if (user.budget >= resultPrice)
            {
                dataBase.openConnection();
                SqlCommand command;
                foreach (var product in mainDataGrid.Items)
                {
                    DesiredProduct row = product as DesiredProduct;
                    if (row != null)
                    {
                        String query = $"insert into [order](time_order,login_user,name_product,count_purchase) values(@time, @login, @name_product, @count)";

                        command = new SqlCommand(query, dataBase.getConnection());
                        command.Parameters.AddWithValue("@time", DateTime.Now);
                        command.Parameters.AddWithValue("@login", user.login);
                        command.Parameters.AddWithValue("@name_product", row.nameProduct);
                        command.Parameters.AddWithValue("@count", row.desiredCount);
                        command.ExecuteNonQuery();
                    }
                }
                
                user.budget -= resultPrice;

                String queryUpdateBudget = $"update user_info set budget=@budget where login_user=@login";
                command = new SqlCommand(queryUpdateBudget, dataBase.getConnection());
                command.Parameters.AddWithValue("@budget", user.budget);
                command.Parameters.AddWithValue("@login", user.login);
                command.ExecuteNonQuery();

                dataBase.closeConnection();

                budgetUser.Text = user.budget + " руб.";
                ResultPriceValue.Text = " руб.";

                mainDataGrid.ItemsSource = null;
                user.basket.Clear();

                MessageBox.Show("Поздравляем с приобретением, заходите еще!","Поздравление",MessageBoxButton.OK,MessageBoxImage.Asterisk);
            }
        }

        private void mainDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {
                Product item = (Product)dataGrid.SelectedItem;

                Product currentProduct = products.FirstOrDefault(product => product.nameProduct.Equals(item.nameProduct))
                    ?? throw new ArgumentException("Продукт не был найден");

                ProductCardUser productCard = new ProductCardUser(user, currentProduct);
                productCard.ShowDialog();

                updateDataOnWindow();
            }
        }
    }
}
