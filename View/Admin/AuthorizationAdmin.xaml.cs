
using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using Kurs.model;
using Kurs.DataBase;

namespace Kurs.View.Admin
{
    /// <summary>
    /// Interaction logic for AuthorizationAdmin.xaml
    /// </summary>
    public partial class AuthorizationAdmin : Window
    {
        private readonly DatabaseManager databaseManager;

        public AuthorizationAdmin(DatabaseManager databaseManager)
        {
            InitializeComponent();
            this.databaseManager = databaseManager;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox.Text;
            string password = passwordTextBox.Password;

            DataTable dataTable = GetUserByLoginAndPassword(login, password);

            if (dataTable.Rows.Count == 1)
            {
                model.Admin admin = GetUserFromDataRow(dataTable.Rows[0]);

                MessageBox.Show("Вы успешно вошли", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindowAdmin userWindow = new MainWindowAdmin(admin, databaseManager);
                Close();
                userWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Такого аккаунта не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private DataTable GetUserByLoginAndPassword(string login, string password)
        {
            string query = "SELECT AA.login_admin FROM admin_auth AA WHERE AA.login_admin = @login AND AA.password_admin = @password";
            SqlParameter[] parameters =
            {
                new SqlParameter("@login", SqlDbType.VarChar) {Value = login},
                new SqlParameter("@password", SqlDbType.VarChar) {Value = password}
            };

            return databaseManager.ExecuteQuery(query, parameters);
        }

        private model.Admin GetUserFromDataRow(DataRow row)
        {
            string login = row["login_admin"].ToString();
            return new model.Admin(login);
        }
    }
}
