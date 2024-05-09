using Kurs.DataBase;
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
        private readonly User user;
        private readonly Window latestWindow;
        private readonly DatabaseManager databaseManager;

        public UserLC(User user, Window mainWindow, DatabaseManager databaseManager)
        {
            InitializeComponent();

            this.user = user;
            latestWindow = mainWindow;
            this.databaseManager = databaseManager;

            loginTextBox.Text = user.login;
            roleTextBox.Text = "user";
            budgetTextBox.Text = user.budget.ToString();
        }

        private void addMoneyButton_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(moneyTextBox.Text, out double amount) || amount <= 0)
            {
                MessageBox.Show("Введите положительное число в поле суммы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                double newBudget = user.budget + amount;
                UpdateUserBudget(newBudget);
                user.budget = newBudget;
                moneyTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при пополнении счета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateUserBudget(double newBudget)
        {
            string query = "UPDATE user_info SET budget=@budget WHERE login_user=@login";
            SqlParameter[] parameters =
            {
                new SqlParameter("@budget", SqlDbType.Float) {Value = newBudget},
                new SqlParameter("@login", SqlDbType.VarChar) {Value = user.login}
            };

            databaseManager.ExecuteQuery(query, parameters);

            budgetTextBox.Text = newBudget.ToString();
            MessageBox.Show("Успешное пополнение средств", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            latestWindow.Close();
            Authorization authorization = new Authorization();
            this.Close();
            authorization.ShowDialog();
        }
    }
}
