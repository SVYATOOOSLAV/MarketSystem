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

namespace Kurs
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindowUser : Window
    {
        private User user;
        private DataBase dataBase = new DataBase();
        List<Product> products;
        public MainWindowUser(User user)
        {
            this.user = user;
            InitializeComponent();
            initDataGrid();
        }

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

        private void userInfoButton_Click(object sender, RoutedEventArgs e)
        {
            UserLC userLC = new UserLC(user, this);
            userLC.ShowDialog();
        }

        private void mainDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //todo refactor 
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {
                Product item = (Product)dataGrid.SelectedItem;

                Product currentProduct = getCurrentProduct(products, item.nameProfuct);

                ProductCardUser productCard = new ProductCardUser(user, currentProduct);
                productCard.ShowDialog();

            }
        }

        private Product getCurrentProduct(List<Product> products, String name)
        {
            foreach (Product product in products)
            {
                if (product.nameProfuct.Equals(name))
                {
                    return product;
                }
            }
            throw new KeyNotFoundException("Продукт не был найден");
        }
    }
}
