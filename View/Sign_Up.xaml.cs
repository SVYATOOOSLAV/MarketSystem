using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Data;
using Kurs.DataBase;

namespace Kurs
{
    /// <summary>
    /// Interaction logic for Sign_Up.xaml
    /// </summary>
    public partial class Sign_Up : Window
    {
        private readonly DatabaseManager databaseManager;

        public Sign_Up()
        {
            InitializeComponent();
            String dataBaseConn = @"Data Source=SVYATBOOK;Initial Catalog=kurs;Integrated Security=True";
            databaseManager = new DatabaseManager(dataBaseConn);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox.Text;
            string password = passwordTextBox.Password;

            if (!ValidateRegistration(login, password))
            {
                return;
            }

            if (CreateUser(login, password))
            {
                MessageBox.Show("Аккаунт создан", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBox.Show("Аккаунт не создан", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CreateUser(string login, string password)
        {
            try
            {
                databaseManager.openConnection();

                string queryAuth = "INSERT INTO user_auth (login_user, password_user) VALUES (@login, @password)";
                
                SqlParameter[] parametersAuth =
                {
                new SqlParameter("@login", SqlDbType.VarChar) {Value = login},
                new SqlParameter("@password", SqlDbType.VarChar) {Value = password}
                };

                databaseManager.ExecuteQuery(queryAuth, parametersAuth);

                string queryInfo = "INSERT INTO user_info (login_user, budget) VALUES (@login, 0)";
               
                SqlParameter[] parametersInfo =
                {
                new SqlParameter("@login", SqlDbType.VarChar) {Value = login}
                };
               
                databaseManager.ExecuteQuery(queryInfo, parametersInfo);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании аккаунта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                databaseManager.closeConnection();
            }
        }

        private bool ValidateRegistration(string login, string password)
        {
            // Проверка наличия логина и пароля
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Проверка длины логина и пароля
            if (login.Length < 4 || password.Length < 6)
            {
                MessageBox.Show("Логин должен содержать не менее 4 символов, а пароль - не менее 6 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Проверка на наличие специальных символов в логине
            if (login.Any(char.IsPunctuation))
            {
                MessageBox.Show("Логин не должен содержать специальные символы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Проверка на наличие специальных символов в пароле
            if (password.Any(char.IsPunctuation))
            {
                MessageBox.Show("Пароль не должен содержать специальные символы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Проверка уникальности логина
            if (isUserExist(login))
            {
                return false;
            }

            return true;
        }

        private bool isUserExist(string login)
        {
            string query = "SELECT COUNT(*) FROM user_auth WHERE login_user = @login";
            SqlParameter parameter = new SqlParameter("@login", SqlDbType.VarChar) { Value = login };

            DataTable dataTable = databaseManager.ExecuteQuery(query, new[] { parameter });
            int userCount = Convert.ToInt32(dataTable.Rows[0][0]);

            if (userCount > 0)
            {
                MessageBox.Show("Аккаунт уже существует", "Ошибка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                return true;
            }

            return false;
        }
    }
}