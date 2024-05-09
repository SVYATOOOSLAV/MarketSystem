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
using Kurs.View.admin;
using Kurs.DataBase;
using System.Collections.ObjectModel;

namespace Kurs.View.Admin
{
    /// <summary>
    /// Interaction logic for MainWindowAdmin.xaml
    /// </summary>
    public partial class MainWindowAdmin : Window
    {
        private model.Admin admin;
        private DatabaseManager dataBaseManager;
        private List<Product> products = new List<Product>();

        public MainWindowAdmin(model.Admin admin, DatabaseManager databaseManager)
        {
            InitializeComponent();
            this.admin = admin;
            this.dataBaseManager = databaseManager; 
            InitDataGrid();
        }

        private async void InitDataGrid()
        {
            try
            {
                dataBaseManager.openConnection();
                DataTable dataTable = await Task.Run(() => dataBaseManager.ExecuteQuery("select [type], [name], [description], [cost], numberForPurchase from product"));

                products = GetProductList(dataTable);
                mainDataGrid.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                dataBaseManager.closeConnection();
            }
        }

        private List<Product> GetProductList(DataTable dataTable)
        {
            List<Product> productList = new List<Product>();
            foreach (DataRow row in dataTable.Rows)
            {
                TypeProduct type = (TypeProduct)Enum.Parse(typeof(TypeProduct), row["type"].ToString(), true);
                string name = row["name"].ToString();
                string description = row["description"].ToString();
                double cost = Convert.ToDouble(row["cost"]);
                int countForPurchase = Convert.ToInt32(row["numberForPurchase"]);

                productList.Add(new Product(type, name, description, cost, countForPurchase));
            }
            return productList;
        }

        private void mainDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {
                Product selectedProduct = (Product)dataGrid.SelectedItem;
                Product currentProduct = products.FirstOrDefault(product => product.nameProduct.Equals(selectedProduct.nameProduct))
                    ?? throw new ArgumentException("Продукт не был найден");

                ProductCardAdmin productCardAdmin = new ProductCardAdmin(currentProduct, products, dataBaseManager);
                productCardAdmin.ShowDialog();

                mainDataGrid.ItemsSource = null;
                mainDataGrid.ItemsSource = products;
            }
        }

        private void CreateItemButton_Click(object sender, RoutedEventArgs e)
        {
            CreateProductWindow createProductWindow = new CreateProductWindow(products, dataBaseManager);
            createProductWindow.ShowDialog();
            mainDataGrid.Items.Refresh();
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            Authorization authorization = new Authorization();
            Close();
            authorization.ShowDialog();
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow(admin, dataBaseManager);
            orderWindow.ShowDialog();
        }
    }
}
