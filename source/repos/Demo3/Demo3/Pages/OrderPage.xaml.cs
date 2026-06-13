using Demo3.DBContext;
using Demo3.Windows;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace Demo3.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        public OrderPage()
        {
            InitializeComponent();

            RefreshData();

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddOrderWindow();
            addWindow.ShowDialog();

            RefreshData();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = MainListView.SelectedItem as Order;

            if (selectedItem == null) return;

            var mbox = MessageBox.Show("Вы точно хотите удалить данный заказ?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (mbox == MessageBoxResult.Yes)
            {
                DataBaseConnection.demoEntities.Order.Remove(selectedItem);
                DataBaseConnection.demoEntities.SaveChanges();
            }

            RefreshData();
        }

        private void MainListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = MainListView.SelectedItem as Order;

            if (selectedItem == null) return;

            var addWindow = new EditOrderWindow(selectedItem);
            addWindow.ShowDialog();

            RefreshData();
        }

        private void RefreshData()
        {
            MainListView.ItemsSource = DataBaseConnection.demoEntities.Order.OrderBy(x => x.ID)
                .Include("Address")
                .Include("Status")
                .Include("User")
                .ToList();
        }
    }
}
