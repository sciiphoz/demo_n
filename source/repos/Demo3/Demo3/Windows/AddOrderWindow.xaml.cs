using Demo3.DBContext;
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
    /// Логика взаимодействия для AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        public AddOrderWindow()
        {
            InitializeComponent();

            AddressCB.ItemsSource = DataBaseConnection.demoEntities.Address.OrderBy(x => x.ID).Select(x => x.Name).ToList();
            ArticleCB.ItemsSource = DataBaseConnection.demoEntities.Product.OrderBy(x => x.ID).Select(x => x.Article).ToList();
            StatusCB.ItemsSource = DataBaseConnection.demoEntities.Status.OrderBy(x => x.ID).Select(x => x.Name).ToList();

            AddressCB.SelectedIndex = 0;
            ArticleCB.SelectedIndex = 0;
            StatusCB.SelectedIndex = 0;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newOrder = new Order
                {
                    ID_Address = AddressCB.SelectedIndex + 1,
                    ID_Status = AddressCB.SelectedIndex + 1,
                    OrderDate = OrderDateDP.SelectedDate,
                    ShipmentDate = ShipmentDateDP.SelectedDate,
                    Code = DataBaseConnection.demoEntities.Order.Max(x => x.Code) + 1,
                    ID_User = DataBaseConnection.demoEntities.User.Max(x => x.ID)
                };

                DataBaseConnection.demoEntities.Order.Add(newOrder);
                DataBaseConnection.demoEntities.SaveChanges();

                var newOrderProduct = new Order_Products 
                {
                    ID_Order = DataBaseConnection.demoEntities.Product.Max(x => x.ID),
                    ID_Product = ArticleCB.SelectedIndex + 1,
                    Quantity = 1
                };

                DataBaseConnection.demoEntities.Order_Products.Add(newOrderProduct);
                DataBaseConnection.demoEntities.SaveChanges();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
