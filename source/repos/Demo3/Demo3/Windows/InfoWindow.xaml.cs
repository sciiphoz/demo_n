using Demo3.DBContext;
using Demo3.Pages;
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

namespace Demo3.Windows
{
    /// <summary>
    /// Логика взаимодействия для InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();

            MainFrame.Content = new ProductPage();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            CurrentUser.currentUser = null;
            this.Close();
        }

        private void ProductButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ProductPage();
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new OrderPage();
        }
    }
}
