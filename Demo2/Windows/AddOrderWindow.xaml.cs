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
using System.Windows.Shapes;

namespace Demo2.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        public AddOrderWindow()
        {
            InitializeComponent();

            AddressCB.ItemsSource = DataBaseConnection.demoEntities.Address.OrderBy(x => x.ID).Select(x => x.Name).ToList();
            ArticleCB.ItemsSource = DataBaseConnection.demoEntities.Product.OrderBy(x => x.ID).Select(x => x.Article).ToList();
            UserCB.ItemsSource = DataBaseConnection.demoEntities.User.OrderBy(x => x.ID).Select(x => x.FullName).ToList();
            StatusCB.ItemsSource = DataBaseConnection.demoEntities.Status.OrderBy(x => x.ID).Select(x => x.Name).ToList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var newOrder = new Order
                {
                    OrderDate = OrderDateDP.SelectedDate,
                    ShipmentDate = ShipmentDateDP.SelectedDate,
                    ID_User = UserCB.SelectedIndex + 1,
                    ID_Status = StatusCB.SelectedIndex + 1,
                    ID_Address = AddressCB.SelectedIndex + 1,
                    Code = DataBaseConnection.demoEntities.Order.Max(x => x.Code) + 1,
                };

                DataBaseConnection.demoEntities.Order.Add(newOrder);

                var newOrderProduct = new Order_Products
                {
                    ID_Product = ArticleCB.SelectedIndex + 1,
                    ID_Order = DataBaseConnection.demoEntities.Order.Max(x => x.ID),
                    Quantity = 1
                };

                DataBaseConnection.demoEntities.Order_Products.Add(newOrderProduct);
                DataBaseConnection.demoEntities.SaveChanges();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
