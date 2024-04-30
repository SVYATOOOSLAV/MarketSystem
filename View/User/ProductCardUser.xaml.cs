using Kurs.model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Interaction logic for ProductCardUser.xaml
    /// </summary>
    public partial class ProductCardUser : Window
    {
        private User user;
        private Product product;
        private DataBase dataBase = new DataBase();
        private SqlDataAdapter adapter = new SqlDataAdapter();
        private DataTable dataTable = new DataTable();

        public ProductCardUser(User user, Product product)
        {
            InitializeComponent();
            this.user = user;
            this.product = product;

            addContentOnWindow();
            var desiredCount = user.basket.FirstOrDefault(pair => pair.Key.nameProfuct.Equals(nameProduct)).Value;
            countForPurchaseTextBox.Text = desiredCount.ToString();
        }

        private void addContentOnWindow()
        {
            typeProduct.Content = product.typeProduct;
            nameProduct.Content = product.nameProfuct;
            TextRange textRange = new TextRange(descriptionProduct.Document.ContentStart, descriptionProduct.Document.ContentEnd);
            textRange.Text = product.descriptionProfuct;
            costProduct.Content = product.costProduct;
            numberForPurchase.Content = product.numberForPurchase;
        }

        private void plusButton_Click(object sender, RoutedEventArgs e)
        {
            int currentCount = int.Parse(countForPurchaseTextBox.Text);

            dataBase.openConnection();

            String query = $"select numberForPurchase from product where name=@nameProduct";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());
            command.Parameters.AddWithValue("@nameProduct", product.nameProfuct);
            adapter.SelectCommand = command;
            adapter.Fill(dataTable);
            int numberForPurchaseInDB = int.Parse(dataTable.Rows[0]["numberForPurchase"].ToString());

            if (numberForPurchaseInDB < currentCount + 1)
            {
                MessageBox.Show("На складе нет столько товара, обратитесь к администартору");
                return;
            }

            countForPurchaseTextBox.Text = (currentCount + 1).ToString();

            query = $"update product set numberForPurchase=@count where product.name=@name";
            command = new SqlCommand(query, dataBase.getConnection());
            command.Parameters.AddWithValue("@count", numberForPurchaseInDB - 1);
            command.Parameters.AddWithValue("@name", product.nameProfuct);
            command.ExecuteNonQuery();

            user.addProductToBasket(product);

            dataBase.closeConnection();
        }
    }
}
