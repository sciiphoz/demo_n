using Demo2.DataBaseContext;
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

namespace Demo2.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            User currentUser = DataBaseConnection.demoEntities.User.FirstOrDefault(x => x.ID == 11);
            CurrentUser.currentUser = currentUser;

            if (currentUser == null) return;

            var parent = Window.GetWindow(this);
            var infoWindow = new InfoWindow();
            infoWindow.Show();
            parent.Close();
        }
    }
}
