using Kurs.enums;
using Kurs.model;
using Kurs.View.Admin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kurs
{
    public partial class Authorization : Window
    {
        DataBase dataBase = new DataBase();
        public Authorization()
        {
            InitializeComponent();
            dataBase.openConnection();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String login = loginTextBox.Text;
            String password = passwordTextBox.Password;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();

            String query = $"select UA.login_user, UI.budget " +
                $"from user_auth UA left join user_info UI on UA.login_user = UI.login_user " +
                $"where UA.login_user=@login and UA.password_user=@password";

            SqlCommand command = new SqlCommand(query, dataBase.getConnection());
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@password", password);

            adapter.SelectCommand = command;
            adapter.Fill(dataTable);
            dataBase.closeConnection();

            if (dataTable.Rows.Count == 1)
            {
                User user = getUserFromDB(dataTable);

                MessageBox.Show("Вы успешно вошли", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindowUser userWindow = new MainWindowUser(user);
                this.Close();
                userWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Такого аккаунта не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private User getUserFromDB(DataTable dataTable)
        {
            DataRow row = dataTable.Rows[0];
            String userLogin = row["login_user"].ToString();
            Double budget = Convert.ToDouble(row["budget"].ToString());

            return new User(userLogin, budget);
        }

        private void createAcc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Sign_Up sign = new Sign_Up();
            sign.ShowDialog();
        }

        private void adminAuth_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AuthorizationAdmin auth = new AuthorizationAdmin(); 
            this.Close(); 
            auth.ShowDialog();
        }
    }
}
