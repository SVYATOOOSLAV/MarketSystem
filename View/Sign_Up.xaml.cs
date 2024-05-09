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

namespace Kurs
{
    /// <summary>
    /// Interaction logic for Sign_Up.xaml
    /// </summary>
    public partial class Sign_Up : Window
    {
        public Sign_Up()
        {
            InitializeComponent();
        }
        DataBase dataBase = new DataBase();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dataBase.openConnection();

            String login = loginTextBox.Text;
            String password = passwordTextBox.Text;

            if(login.Trim() == "" || password.Trim() == "")
            {
                MessageBox.Show("Невозможно создать пользователя","Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка введенных данных
            if (!ValidateRegistration(login, password))
            {
                return;
            }

            String query = $"insert into user_auth(login_user, password_user) values('{login}','{password}')";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт создан", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Аккаунт не создан");
            }

            query = $"insert into user_info(login_user,budget) values('{login}',0)";
            command = new SqlCommand(query, dataBase.getConnection());
            command.ExecuteNonQuery();

            dataBase.closeConnection();
        }

        private Boolean isUserExist(String login)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();

            String query = $"select * from user_auth where login_user='{login}'";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(dataTable);


            if (dataTable.Rows.Count > 0)
            {
                MessageBox.Show("Аккаунт уже существует", "Ошибка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                return true;
            }
            return false;
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
    }
}
