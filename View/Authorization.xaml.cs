using Kurs.DataBase;
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
        private readonly DatabaseManager databaseManager;

        public Authorization()
        {
            InitializeComponent();
            String connectionString = @"Data Source=SVYATBOOK;Initial Catalog=kurs;Integrated Security=True";
            databaseManager = new DatabaseManager(connectionString);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox.Text;
            string password = passwordTextBox.Password;

            string query = "SELECT UA.login_user, UI.budget " +
                           "FROM user_auth UA LEFT JOIN user_info UI ON UA.login_user = UI.login_user " +
                           "WHERE UA.login_user = @login AND UA.password_user = @password";

            SqlParameter[] parameters =
            {
                new SqlParameter("@login", SqlDbType.VarChar) {Value = login},
                new SqlParameter("@password", SqlDbType.VarChar) {Value = password}
            };

            DataTable dataTable = databaseManager.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count == 1)
            {
                User user = GetUserFromDataRow(dataTable.Rows[0]);

                MessageBox.Show("Вы успешно вошли", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindowUser userWindow = new MainWindowUser(user, databaseManager);
                Close();
                userWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Такого аккаунта не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private User GetUserFromDataRow(DataRow row)
        {
            string userLogin = row["login_user"].ToString();
            double budget = Convert.ToDouble(row["budget"]);

            return new User(userLogin, budget);
        }


        private void createAcc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Sign_Up sign = new Sign_Up();
            sign.ShowDialog();
        }

        private void adminAuth_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AuthorizationAdmin auth = new AuthorizationAdmin(databaseManager);
            this.Close();
            auth.ShowDialog();
        }
    }
}
