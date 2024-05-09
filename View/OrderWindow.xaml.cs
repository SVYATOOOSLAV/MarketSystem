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
using Kurs.DataBase;

namespace Kurs.View
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private readonly DatabaseManager databaseManager;
        private List<Order> orders;

        public OrderWindow(User user, DatabaseManager databaseManager)
        {
            InitializeComponent();
            this.databaseManager = databaseManager;
            InitializeDataGrid(user);
        }

        public OrderWindow(model.Admin admin, DatabaseManager databaseManager)
        {
            InitializeComponent();
            this.databaseManager = databaseManager;
            InitializeDataGrid(admin);
        }

        private void InitializeDataGrid(object user)
        {
            string query;
            if (user is User)
            {
                User us = (User)user;
                query = $"SELECT time_order, login_user, name_product, count_purchase FROM [order] WHERE login_user='{us.login}'";
            }
            else
            {
                query = $"SELECT time_order, login_user, name_product, count_purchase FROM [order]";
            }

            DataTable dataTable = databaseManager.ExecuteQuery(query);
            orders = GetOrderList(dataTable);
            mainDataGrid.ItemsSource = orders;
        }

        private List<Order> GetOrderList(DataTable dataTable)
        {
            List<Order> orders = new List<Order>();
            foreach (DataRow row in dataTable.Rows)
            {
                DateTime time = DateTime.Parse(row["time_order"].ToString());
                string login = row["login_user"].ToString();
                string name = row["name_product"].ToString();
                int countForPurchase = Convert.ToInt32(row["count_purchase"]);

                orders.Add(new Order(time, login, name, countForPurchase));
            }

            return orders;
        }
    }
}
