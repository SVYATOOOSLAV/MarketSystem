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
using System.ComponentModel;

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
        private int numberForPurchaseInDB;
        private int stateAfterSelectProduct;

        public ProductCardUser(User user, Product product)
        {
            InitializeComponent();
            this.user = user;
            this.product = product;
            addContentOnWindow();

            var desiredCount = user.basket.FirstOrDefault(pair => pair.Key.nameProduct.Equals(product.nameProduct)).Value;
            countForPurchaseTextBox.Text = desiredCount.ToString();
            stateAfterSelectProduct = desiredCount;

            updateNumberForPurchaseInDB();
            Closing += ProductCardUser_Closing;
        }

        private void ProductCardUser_Closing(object sender, CancelEventArgs e)
        {
            // update DB after close window
            dataBase.openConnection();
            int currentCount = int.Parse(countForPurchaseTextBox.Text);
            String query = $"update product set numberForPurchase=@count where product.name=@name";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());

            if (stateAfterSelectProduct > 0 && stateAfterSelectProduct < currentCount)
            {
                command.Parameters.AddWithValue("@count", numberForPurchaseInDB + stateAfterSelectProduct - currentCount);
            }
            else if (stateAfterSelectProduct < currentCount)
            {
                command.Parameters.AddWithValue("@count", numberForPurchaseInDB - currentCount);
            }
            else
            {
                command.Parameters.AddWithValue("@count", numberForPurchaseInDB + stateAfterSelectProduct - currentCount);
            }

            command.Parameters.AddWithValue("@name", product.nameProduct);
            command.ExecuteNonQuery();
            dataBase.closeConnection();
        }

        private void addContentOnWindow()
        {
            typeProduct.Content = product.typeProduct;
            nameProduct.Content = product.nameProduct;
            TextRange textRange = new TextRange(descriptionProduct.Document.ContentStart, descriptionProduct.Document.ContentEnd);
            textRange.Text = product.descriptionProduct;
            costProduct.Content = product.costProduct;
            numberForPurchase.Content = product.numberForPurchase;
        }

        private void updateNumberForPurchaseInDB()
        {
            dataBase.openConnection();
            String query = $"select numberForPurchase from product where name=@nameProduct";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());
            command.Parameters.AddWithValue("@nameProduct", product.nameProduct);
            adapter.SelectCommand = command;
            adapter.Fill(dataTable);
            numberForPurchaseInDB = int.Parse(dataTable.Rows[0]["numberForPurchase"].ToString());
            dataBase.closeConnection();
        }

        private void plusButton_Click(object sender, RoutedEventArgs e)
        {
            int currentCount = int.Parse(countForPurchaseTextBox.Text);

            if (numberForPurchaseInDB < currentCount + 1)
            {
                MessageBox.Show("На складе нет столько товара, обратитесь к администартору");
                return;
            }

            countForPurchaseTextBox.Text = (currentCount + 1).ToString();

            user.addProductToBasket(product);
            product.numberForPurchase--;

            numberForPurchase.Content = (product.numberForPurchase).ToString();
        }

        private void minusButton_Click(object sender, RoutedEventArgs e)
        {
            int currentCount = int.Parse(countForPurchaseTextBox.Text);

            if (currentCount - 1 < 0)
            {
                MessageBox.Show("Выбрать отрицательное количество товаров нельзя");
                return;
            }

            countForPurchaseTextBox.Text = (currentCount - 1).ToString();

            user.removeProductFromBasket(product);
            product.numberForPurchase++;

            numberForPurchase.Content = (product.numberForPurchase).ToString();
        }
    }
}
