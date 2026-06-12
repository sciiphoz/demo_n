using Demo2.DataBaseContext;
using Demo2.Pages;
using Demo2.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demo2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Content = new AuthPage();
        }
        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            User currentUser = new User
            {
                FullName = "Гость",
                Login = " ",
                ID_Role = 0
            };

            CurrentUser.currentUser = currentUser;

            var parent = Window.GetWindow(this);
            var infoWindow = new InfoWindow();
            infoWindow.Show();
            parent.Close();
        }
    }
}
