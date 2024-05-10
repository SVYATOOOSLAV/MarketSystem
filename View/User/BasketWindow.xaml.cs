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
using Kurs.DataBase;

namespace Kurs.View.user
{
    /// <summary>
    /// Interaction logic for Basket.xaml
    /// </summary>
    public partial class BasketWindow : Window
    {
        private readonly User user;
        private readonly List<Product> products;
        private readonly DatabaseManager databaseManager;
        private List<DesiredProduct> availableProducts;
        private double resultPrice;

        public BasketWindow(User user, List<Product> products, DatabaseManager databaseManager)
        {
            InitializeComponent();
            this.user = user;
            this.products = products;
            this.databaseManager = databaseManager;

            userLabel.Content += user.login;

            UpdateDataOnWindow();
        }

        private void UpdateDataOnWindow()
        {
            ShowBasketUser();
            UpdateResultPrice();
            budgetUser.Text = $"{user.budget} руб.";
        }

        private void ShowBasketUser()
        {
            availableProducts = user.basketManager.getBasket().Select(product =>
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

        private void UpdateResultPrice()
        {
            resultPrice = availableProducts.Sum(product => product.costProduct * product.desiredCount);
            ResultPriceValue.Text = $"{resultPrice} руб.";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserLC lc = new UserLC(user, this, databaseManager);
            lc.ShowDialog();
            budgetUser.Text = $"{user.budget} руб.";
        }

        private void PurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (user.budget < resultPrice)
            {
                MessageBoxResult result = MessageBox.Show("У вас недостаточно средств, пополните их", "Информирование", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                {
                    Button_Click(sender, e);
                }
            }
            else
            {
                try
                {
                    PurchaseProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при покупке товаров: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void PurchaseProducts()
        {
            try
            {
                databaseManager.BeginTransaction();

                foreach (var product in availableProducts)
                {
                    string query = "INSERT INTO [order] (time_order, login_user, name_product, count_purchase) VALUES (@time, @login, @name_product, @count)";
                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@time", SqlDbType.DateTime) {Value = DateTime.Now},
                        new SqlParameter("@login", SqlDbType.VarChar) {Value = user.login},
                        new SqlParameter("@name_product", SqlDbType.VarChar) {Value = product.nameProduct},
                        new SqlParameter("@count", SqlDbType.Int) {Value = product.desiredCount}
                    };
                    databaseManager.ExecuteQuery(query, parameters);
                }

                user.budget -= resultPrice;
                string queryUpdateBudget = "UPDATE user_info SET budget=@budget WHERE login_user=@login";
                SqlParameter[] parametersUpdateBudget =
                {
                    new SqlParameter("@budget", SqlDbType.Float) {Value = user.budget},
                    new SqlParameter("@login", SqlDbType.VarChar) {Value = user.login}
                };
                databaseManager.ExecuteQuery(queryUpdateBudget, parametersUpdateBudget);

                databaseManager.CommitTransaction();

                user.basketManager.getBasket().Clear();
                ShowBasketUser();

                MessageBox.Show("Поздравляем с приобретением, заходите еще!", "Поздравление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (Exception ex)
            {
                databaseManager.RollbackTransaction();

                MessageBox.Show($"An error occurred while purchasing products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                UpdateDataOnWindow();
            }
        }

        private void mainDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {
                Product item = (Product)dataGrid.SelectedItem;
                Product currentProduct = products.FirstOrDefault(product => product.nameProduct.Equals(item.nameProduct));
                if (currentProduct != null)
                {
                    ProductCardUser productCard = new ProductCardUser(user, currentProduct, databaseManager);
                    productCard.ShowDialog();
                    UpdateDataOnWindow();
                }
            }
        }
    }
}
