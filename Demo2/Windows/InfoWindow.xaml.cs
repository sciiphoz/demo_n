using Demo2.DataBaseContext;
using Demo2.Pages;
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

namespace Demo2.Windows
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
            
            LogoImage.Source = new BitmapImage(new Uri("C:\\Users\\sciiphoz\\source\\repos\\Demo2\\Demo2\\bin\\Debug\\Media\\Logo\\icon.png", UriKind.Absolute));

            UserName.Text = CurrentUser.currentUser.FullName;
            UserLogin.Text = CurrentUser.currentUser.Login;
            UserRole.Text = CurrentUser.currentUser.ID_Role == 0 ? "Гость" : CurrentUser.currentUser.Role.Name;

            if (CurrentUser.currentUser.ID_Role == 0 || CurrentUser.currentUser.ID_Role == 3)
            {
                OrderButton.Visibility = Visibility.Collapsed;
            }

        }

        private void ProductButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ProductPage();
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new OrderPage();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            CurrentUser.currentUser = null;
            this.Close();
        }
    }
}
