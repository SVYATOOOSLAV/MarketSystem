﻿using Kurs.enums;
using Kurs.model;
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

            String query = $"select id_user, login_user, password_user, role from user_auth where login_user='{login}' and password_user='{password}'";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(dataTable);
            dataBase.closeConnection();

            if (dataTable.Rows.Count == 1)
            {
                User user = getUserFromDB(dataTable);
                user.budget = getUserBudget(login);

                MessageBox.Show("Вы успешно вошли", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow win1 = new MainWindow(user);
                this.Close();
                win1.ShowDialog();
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
            String userPassword = row["password_user"].ToString();
            String userRole = row["role"].ToString();

            return new User(userLogin, userPassword, userRole);
        }

        private Double getUserBudget(String login)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();

            String query = "select budget from user_info where id_user in (Select id_user from user_auth where login_user=@login)";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());
            command.Parameters.AddWithValue("@login", login);
            adapter.SelectCommand = command;
            adapter.Fill(dataTable);

           return Double.Parse(dataTable.Rows[0]["budget"].ToString());
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Sign_Up sign = new Sign_Up();
            sign.Show();
            this.Hide();
        }
    }
}