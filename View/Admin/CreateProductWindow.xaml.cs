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
        private DataBase dataBase = new DataBase();

        private List<TypeProduct> typeProducts = new List<TypeProduct>
        {
                TypeProduct.IPHONE,
                TypeProduct.ANDROID
        };

        private List<Product> products;

        public CreateProductWindow(List<Product> products)
        {
            InitializeComponent();

            typeProductBox.ItemsSource = typeProducts;
            typeProductBox.SelectedIndex = 1;

            this.products = products;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(descriptionProduct.Document.ContentStart, descriptionProduct.Document.ContentEnd);
            string text = textRange.Text;

            dataBase.openConnection();
            String query = $"insert into product(type,name,description,cost,numberForPurchase) values (@type, @name, @description, @cost, @count)";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());

            command.Parameters.AddWithValue("@type", typeProductBox.SelectedItem);
            command.Parameters.AddWithValue("@name", nameProductTextBox.Text);
            command.Parameters.AddWithValue("@description", text);
            command.Parameters.AddWithValue("@cost", double.Parse(priceProductTextBox.Text));
            command.Parameters.AddWithValue("@count", int.Parse(numberForPurchaseTextBox.Text));

            command.ExecuteNonQuery();
            dataBase.closeConnection();

            products.Add(new Product(
                (TypeProduct)typeProductBox.SelectedItem,
                nameProductTextBox.Text,
                text,
                double.Parse(priceProductTextBox.Text),
                int.Parse(numberForPurchaseTextBox.Text))
            );

            MessageBox.Show("Продукт успещно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
