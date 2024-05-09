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
    /// Interaction logic for ProductCardAdmin.xaml
    /// </summary>
    public partial class ProductCardAdmin : Window
    {
        private Product product;
        private List<Product> products;
        private DatabaseManager dataBaseManager;

        public ProductCardAdmin(Product product, List<Product> products, DatabaseManager databaseManager)
        {
            InitializeComponent();
            this.product = product;
            this.products = products;
            this.dataBaseManager = databaseManager;
            AddContentOnWindow();
        }

        private void AddContentOnWindow()
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
            string newName = nameProductTextBox.Text;
            string description = new TextRange(descriptionProduct.Document.ContentStart, descriptionProduct.Document.ContentEnd).Text;
            double cost;
            int count;

            if (!double.TryParse(priceProductTextBox.Text, out cost) || !int.TryParse(numberForPurchaseTextBox.Text, out count))
            {
                MessageBox.Show("Invalid price or number for purchase.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                dataBaseManager.openConnection();

                string query = "UPDATE product SET name=@newName, description=@description, cost=@cost, numberForPurchase=@count WHERE name=@oldName";
                SqlCommand command = new SqlCommand(query, dataBaseManager.getConnection());

                command.Parameters.AddWithValue("@newName", newName);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@cost", cost);
                command.Parameters.AddWithValue("@count", count);
                command.Parameters.AddWithValue("@oldName", product.nameProduct);

                command.ExecuteNonQuery();

                product.nameProduct = newName;
                product.descriptionProduct = description;
                product.costProduct = cost;
                product.numberForPurchase = count;

                AddContentOnWindow();

                MessageBox.Show("Data updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                dataBaseManager.closeConnection();
            }
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataBaseManager.openConnection();
                string query = "DELETE FROM product WHERE name=@name";
                SqlCommand command = new SqlCommand(query, dataBaseManager.getConnection());
                command.Parameters.AddWithValue("@name", product.nameProduct);
                command.ExecuteNonQuery();
                products.Remove(product);
                MessageBox.Show("Product deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                dataBaseManager.closeConnection();
            }
        }
    }
}
