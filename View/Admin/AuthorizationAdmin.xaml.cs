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

namespace Kurs.View.Admin
{
    /// <summary>
    /// Interaction logic for AuthorizationAdmin.xaml
    /// </summary>
    public partial class AuthorizationAdmin : Window
    {
        DataBase dataBase = new DataBase();
        public AuthorizationAdmin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String login = loginTextBox.Text;
            String password = passwordTextBox.Password;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();

            String query = $"select AA.login_admin from admin_auth AA " +
                $"where AA.login_admin=@login and AA.password_admin=@password";

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
    }
}
