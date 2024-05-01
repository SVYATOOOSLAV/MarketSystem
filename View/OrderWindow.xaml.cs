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

namespace Kurs.View
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private DataBase dataBase = new DataBase();
        private List<Order> orders;

        public OrderWindow(User user)
        {
            InitializeComponent();

            initDataGrid(user);
        }

        public OrderWindow(model.Admin admin)
        {
            InitializeComponent();

            initDataGrid(admin);
        }


        private void initDataGrid(Object user)
        {
            dataBase.openConnection();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();

            String query;
            if (user is User)
            {
                User us = (User)user;
                query = $"select time_order, login_user, name_product, count_purchase from [order] where login_user='{us.login}'";
            }
            else
            {
                query = $"select time_order, login_user, name_product, count_purchase from [order]";
            }

            SqlCommand command = new SqlCommand(query, dataBase.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(dataTable);
            dataBase.closeConnection();

            orders = getListOrder(dataTable);
            mainDataGrid.ItemsSource = orders;
        }

        private List<Order> getListOrder(DataTable dataTable)
        {
            List<Order> orders = new List<Order>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];

                DateTime time = DateTime.Parse(row["time_order"].ToString()); ;
                String login = row["login_user"].ToString();
                String name = row["name_product"].ToString();
                int countForPurchase = Convert.ToInt32(row["count_purchase"]);

                orders.Add(new Order(time, login, name, countForPurchase));
            }

            return orders;
        }
    }
}
