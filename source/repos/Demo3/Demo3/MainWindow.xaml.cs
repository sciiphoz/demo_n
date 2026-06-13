using Demo3.DBContext;
using Demo3.Windows;
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

namespace Demo3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.currentUser = new User
            {
                Login = " ",
                FullName = "Гость",
                ID_Role = 0
            };

            var infoWindow = new InfoWindow();
            infoWindow.Show();
            this.Close();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentUser.currentUser = new User
                {
                    Login = " ",
                    FullName = "Гость",
                    ID_Role = 0
                };

                var infoWindow = new InfoWindow();
                infoWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
