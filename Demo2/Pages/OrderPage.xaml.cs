using Demo2.DataBaseContext;
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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        public OrderPage()
        {
            InitializeComponent();

            MainListBox.ItemsSource = DataBaseConnection.demoEntities.Order.Include("Address").Include("Status").Include("Order_Products").ToList();
        }

        private void MainListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = MainListBox.SelectedItem as Order;

            MessageBox.Show(selectedItem.ShipmentDate.ToString(), "Дата доставки", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
