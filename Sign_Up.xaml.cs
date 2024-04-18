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
            
            String login = loginTextBox.Text;
            String password = passwordTextBox.Text;

            String query = $"insert into user_auth(login_user, password_user) values('{login}','{password}')";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());

            if (isUserExist()) 
            {
                return;
            }

            dataBase.openConnection();

            if(command.ExecuteNonQuery() == 1 ) 
            {
                MessageBox.Show("Аккаунт создан", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Authorization main = new Authorization();
                this.Hide();
                main.ShowDialog();
            }
            else
            {
                MessageBox.Show("Аккаунт не создан");
            }

            dataBase.closeConnection();
        }

        private Boolean isUserExist()
        {
            String login = loginTextBox.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();

            String query = $"select * from user_auth where login_user='{login}'";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(dataTable);
            

            if(dataTable.Rows.Count > 0 )
            {
                MessageBox.Show("Аккаунт уже существует", "Ошибка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                return true;
            }
            return false;
        }
    }
}
