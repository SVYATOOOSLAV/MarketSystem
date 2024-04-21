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

namespace Kurs
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User user;
        private DataBase dataBase = new DataBase();
        public MainWindow(User user)
        {
            this.user = user;
            InitializeComponent();

            userInfoButton.Content += user.role == TypeUser.USER.ToString().ToLower() ? " Пользователя" : " Администратора";

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

            List<Product> products = getListProduct(dataTable);
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
    }
}
