
using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using Kurs.model;

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
                model.Admin admin = getUserFromDB(dataTable);

                MessageBox.Show("Вы успешно вошли", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindowAdmin userWindow = new MainWindowAdmin(admin);
                this.Close();
                userWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Такого аккаунта не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private model.Admin getUserFromDB(DataTable dataTable)
        {
            DataRow row = dataTable.Rows[0];
            String login = row["login_admin"].ToString();

            return new model.Admin(login);
        }
    }
}
