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

namespace Kurs.View.Admin
{
    /// <summary>
    /// Interaction logic for MainWindowAdmin.xaml
    /// </summary>
    public partial class MainWindowAdmin : Window
    {
        private model.Admin admin;
        private DataBase dataBase = new DataBase();
        private List<Product> products = new List<Product>();

        public MainWindowAdmin(model.Admin admin)
        {
            InitializeComponent();
            this.admin = admin;

            initDataGrid();
        }

        // refactor, create class in /service
        private void initDataGrid()
        {
            dataBase.openConnection();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();

            String query = $"select [type], [name], [description], [cost], numberForPurchase from product";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(dataTable);
            dataBase.closeConnection();

            products = getListProduct(dataTable);
            mainDataGrid.ItemsSource = products;
        }

        private List<Product> getListProduct(DataTable dataTable)
        {
            List<Product> products = new List<Product>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];

                TypeProduct type = (TypeProduct)Enum.Parse(typeof(TypeProduct), row["type"].ToString(), true);
                String name = row["name"].ToString();
                String description = row["description"].ToString();
                Double cost = Convert.ToDouble(row["cost"]);
                int countForPurchase = Convert.ToInt32(row["numberForPurchase"]);

                products.Add(new Product(type, name, description, cost, countForPurchase));
            }

            return products;
        }

        private void mainDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {
                Product item = (Product)dataGrid.SelectedItem;

                Product currentProduct = products.FirstOrDefault(product => product.nameProduct.Equals(item.nameProduct))
                    ?? throw new ArgumentException("Продукт не был найден");

                ProductCardAdmin productCard = new ProductCardAdmin(currentProduct, products);
                productCard.ShowDialog();

                mainDataGrid.Items.Refresh();
            }
        }

        private void createItemButton_Click(object sender, RoutedEventArgs e)
        {
            CreateProductWindow window = new CreateProductWindow(products);
            window.ShowDialog();
            mainDataGrid.Items.Refresh(); 
        }

        private void logOutButton_Click(object sender, RoutedEventArgs e)
        {
            Authorization authorization = new Authorization();
            this.Close();
            authorization.ShowDialog();
        }

        private void orderButton_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow(admin);
            orderWindow.ShowDialog(); 
        }
    }
}
