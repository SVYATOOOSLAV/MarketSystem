using Kurs.DataBase;
using Kurs.model;
using System;
using System.Collections.Generic;
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
using static System.Net.Mime.MediaTypeNames;

namespace Kurs.View.admin
{
    /// <summary>
    /// Interaction logic for CreateProductWindow.xaml
    /// </summary>
    public partial class CreateProductWindow : Window
    {
        private readonly DatabaseManager databaseManager;
        private readonly List<TypeProduct> typeProducts = new List<TypeProduct>
        {
            TypeProduct.IPHONE,
            TypeProduct.ANDROID
        };
        private readonly List<Product> products;

        public CreateProductWindow(List<Product> products, DatabaseManager databaseManager)
        {
            InitializeComponent();
            typeProductBox.ItemsSource = typeProducts;
            typeProductBox.SelectedIndex = 1;
            this.products = products;
            this.databaseManager = databaseManager;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string typeName = typeProductBox.SelectedItem.ToString();
            string productName = nameProductTextBox.Text;

            if (ProductExists(typeName, productName))
            {
                MessageBox.Show("Товар с таким именем и типом уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string description = new TextRange(descriptionProduct.Document.ContentStart, descriptionProduct.Document.ContentEnd).Text;
            double cost = double.Parse(priceProductTextBox.Text);
            int count = int.Parse(numberForPurchaseTextBox.Text);

            InsertProduct(typeName, productName, description, cost, count);

            products.Add(new Product((TypeProduct)typeProductBox.SelectedItem, productName, description, cost, count));
            this.Close();
        }

        private bool ProductExists(string type, string name)
        {
            string queryCheck = "SELECT COUNT(*) FROM product WHERE type = @type AND name = @name";
            using (SqlCommand commandCheck = new SqlCommand(queryCheck, databaseManager.getConnection()))
            {
                commandCheck.Parameters.AddWithValue("@type", type);
                commandCheck.Parameters.AddWithValue("@name", name);
                databaseManager.openConnection();
                int existingProductsCount = (int)commandCheck.ExecuteScalar();
                return existingProductsCount > 0;
            }
        }

        private void InsertProduct(string type, string name, string description, double cost, int count)
        {
            string query = "INSERT INTO product(type, name, description, cost, numberForPurchase) VALUES (@type, @name, @description, @cost, @count)";
            using (SqlCommand command = new SqlCommand(query, databaseManager.getConnection()))
            {
                command.Parameters.AddWithValue("@type", type);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@cost", cost);
                command.Parameters.AddWithValue("@count", count);
                databaseManager.openConnection();
                command.ExecuteNonQuery();
            }
        }
    }
}