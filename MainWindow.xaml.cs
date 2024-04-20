using Kurs.enums;
using Kurs.model;
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

namespace Kurs
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User user;
        public MainWindow(User user)
        {
            this.user = user;
            InitializeComponent();

            userInfoButton.Content += user.role == TypeUser.USER.ToString().ToLower() ? " Пользователя" : " Администратора";
        }
    }
}
