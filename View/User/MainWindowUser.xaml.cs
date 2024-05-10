using Kurs.enums;
using Kurs.model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Xml.Linq;
using Kurs.View.user;
using System.ComponentModel;
using Kurs.View;
using Kurs.DataBase;

namespace Kurs
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindowUser : Window
    {
        private readonly User user;
        private readonly DatabaseManager databaseManager;
        private List<Product> products;

        public MainWindowUser(User user, DatabaseManager databaseManager)
        {
            this.user = user;
            this.databaseManager = databaseManager;
            InitializeComponent();
            InitializeDataGrid();
            Closing += MainWindowUser_Closing;
        }

        private void MainWindowUser_Closing(object sender, CancelEventArgs e)
        {
            if (user.basketManager.getBasket().Count != 0)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите закрыть приложение?\nВаш заказ пропадет", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true; // Отменяем закрытие окна
                }
                else
                {
                    ReturnUnpurchasedProductsToDatabase();
                }
            }
        }

        private void InitializeDataGrid()
        {
            DataTable dataTable = databaseManager.ExecuteQuery("SELECT [type], [name], [description], [cost], numberForPurchase FROM product");
            products = GetProductList(dataTable);
            mainDataGrid.ItemsSource = products;
        }

        private List<Product> GetProductList(DataTable dataTable)
        {
            List<Product> products = new List<Product>();
            foreach (DataRow row in dataTable.Rows)
            {
                TypeProduct type = (TypeProduct)Enum.Parse(typeof(TypeProduct), row["type"].ToString(), true);
                string name = row["name"].ToString();
                string description = row["description"].ToString();
                double cost = Convert.ToDouble(row["cost"]);
                int countForPurchase = Convert.ToInt32(row["numberForPurchase"]);

                products.Add(new Product(type, name, description, cost, countForPurchase));
            }
            return products;
        }

        private void ReturnUnpurchasedProductsToDatabase()
        {
            databaseManager.openConnection();
            foreach (var item in user.basketManager.getBasket())
            {
                if (item.Value > 0)
                {
                    DataTable dataTable = databaseManager.ExecuteQuery($"SELECT numberForPurchase FROM product WHERE name='{item.Key.nameProduct}'");
                    int numberForPurchase = Convert.ToInt32(dataTable.Rows[0]["numberForPurchase"]);
                    int updatedCount = numberForPurchase + item.Value;
                    databaseManager.ExecuteQuery($"UPDATE product SET numberForPurchase={updatedCount} WHERE name='{item.Key.nameProduct}'");
                }
            }
            databaseManager.closeConnection();
        }

        private void userInfoButton_Click(object sender, RoutedEventArgs e)
        {
            UserLC userLC = new UserLC(user, this, databaseManager);
            userLC.ShowDialog();
        }

        private void mainDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {
                Product item = (Product)dataGrid.SelectedItem;
                Product currentProduct = products.FirstOrDefault(product => product.nameProduct.Equals(item.nameProduct))
                    ?? throw new ArgumentException("Продукт не был найден");
                ProductCardUser productCard = new ProductCardUser(user, currentProduct, databaseManager);
                productCard.ShowDialog();
                mainDataGrid.Items.Refresh();
            }
        }

        private void BasketButton_Click(object sender, RoutedEventArgs e)
        {
            BasketWindow basketWin = new BasketWindow(user, products, databaseManager);
            basketWin.ShowDialog();
            mainDataGrid.Items.Refresh();
        }

        private void orderButton_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow(user, databaseManager);
            orderWindow.ShowDialog();
        }
    }
}
