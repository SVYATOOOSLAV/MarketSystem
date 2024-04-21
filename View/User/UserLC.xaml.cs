using Kurs.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for UserLC.xaml
    /// </summary>
    public partial class UserLC : Window
    {
        private User user;
        private MainWindow latestMainWindow;
        private DataBase dataBase = new DataBase();

        public UserLC(User user, MainWindow mainWindow)
        {
            InitializeComponent();

            this.user = user;
            latestMainWindow = mainWindow;

            loginTextBox.Text = user.login;
            roleTextBox.Text = user.role;
            budgetTextBox.Text = user.budget.ToString();
        }

        private void addMoneyButton_Click(object sender, RoutedEventArgs e)
        {
            dataBase.openConnection();
            SqlDataAdapter adapter = new SqlDataAdapter();  
            DataTable dataTable = new DataTable();  

            String query = "update user_info set budget=@budget where id_user in (Select id_user from user_auth where login_user=@login)";

            SqlCommand command = new SqlCommand(query, dataBase.getConnection());
            command.Parameters.AddWithValue("@budget", user.budget + double.Parse(moneyTextBox.Text));
            command.Parameters.AddWithValue("@login", loginTextBox.Text);
            command.ExecuteNonQuery();

            MessageBox.Show("Успешное поплнение средств", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            
            String query2 = "select budget from user_info where id_user in (Select id_user from user_auth where login_user=@login)";
            command = new SqlCommand(query2, dataBase.getConnection());
            command.Parameters.AddWithValue("@login", loginTextBox.Text);
            adapter.SelectCommand = command;
            adapter.Fill(dataTable);

            DataRow row = dataTable.Rows[0];
            budgetTextBox.Text = row["budget"].ToString();
            user.budget = Double.Parse(row["budget"].ToString());
            moneyTextBox.Clear();
        }
          

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            latestMainWindow.Close();
            Authorization authorization = new Authorization();
            this.Close();
            authorization.ShowDialog();
        }
    }
}
