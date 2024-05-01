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

namespace Kurs.View.admin
{
    /// <summary>
    /// Interaction logic for ProductCardAdmin.xaml
    /// </summary>
    public partial class ProductCardAdmin : Window
    {
        private Product product;
        private DataBase dataBase = new DataBase();

        public ProductCardAdmin(Product product)
        {
            InitializeComponent();
            this.product = product;
            addContentOnWindow();
        }

        private void addContentOnWindow()
        {
            typeProduct.Content = product.typeProduct;
            nameProductTextBox.Text = product.nameProduct;
            TextRange textRange = new TextRange(descriptionProduct.Document.ContentStart, descriptionProduct.Document.ContentEnd);
            textRange.Text = product.descriptionProduct;
            priceProductTextBox.Text = product.costProduct.ToString();
            numberForPurchaseTextBox.Text = product.numberForPurchase.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            String lastName = product.nameProduct;
            TextRange textRange = new TextRange(descriptionProduct.Document.ContentStart, descriptionProduct.Document.ContentEnd);
            string text = textRange.Text;

            dataBase.openConnection();
            String query = $"update product set name=@newName, description=@description, cost=@cost, numberForPurchase=@count where product.name=@lastName";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());

            command.Parameters.AddWithValue("@newName", nameProductTextBox.Text);
            command.Parameters.AddWithValue("@description", text);
            command.Parameters.AddWithValue("@cost", double.Parse(priceProductTextBox.Text));
            command.Parameters.AddWithValue("@count", int.Parse(numberForPurchaseTextBox.Text));
            command.Parameters.AddWithValue("@lastName", lastName);

            command.ExecuteNonQuery();
            dataBase.closeConnection();

            product.nameProduct = nameProductTextBox.Text;
            product.descriptionProduct = text;
            product.costProduct = double.Parse(priceProductTextBox.Text);
            product.numberForPurchase = int.Parse(numberForPurchaseTextBox.Text);

            addContentOnWindow();

            MessageBox.Show("Данные успешно обновлены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
