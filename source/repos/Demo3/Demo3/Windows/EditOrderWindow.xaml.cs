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
    /// Логика взаимодействия для EditOrderWindow.xaml
    /// </summary>
    public partial class EditOrderWindow : Window
    {
        private Order _currentOrder;
        private int productId;
        public EditOrderWindow()
        {
            InitializeComponent();
        }
        public EditOrderWindow(Order order)
        {
            InitializeComponent();

            _currentOrder = order;

            AddressCB.ItemsSource = DataBaseConnection.demoEntities.Address.OrderBy(x => x.ID).Select(x => x.Name).ToList();
            ArticleCB.ItemsSource = DataBaseConnection.demoEntities.Product.OrderBy(x => x.ID).Select(x => x.Article).ToList();
            StatusCB.ItemsSource = DataBaseConnection.demoEntities.Status.OrderBy(x => x.ID).Select(x => x.Name).ToList();

            productId = DataBaseConnection.demoEntities.Order_Products.OrderBy(x => x.ID).Where(x => x.ID_Order == _currentOrder.ID).Select(x => x.ID_Product).Max();

            AddressCB.SelectedIndex = _currentOrder.ID_Address - 1;
            ArticleCB.SelectedIndex = productId - 1;
            StatusCB.SelectedIndex = _currentOrder.ID_Address - 1;

            OrderDateDP.SelectedDate = _currentOrder.OrderDate;
            ShipmentDateDP.SelectedDate = _currentOrder.ShipmentDate;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _currentOrder.OrderDate = OrderDateDP.SelectedDate;
                _currentOrder.ShipmentDate = ShipmentDateDP.SelectedDate;

                _currentOrder.ID_Address = AddressCB.SelectedIndex + 1;
                _currentOrder.ID_Status = StatusCB.SelectedIndex + 1;

                Order_Products orderProduct = DataBaseConnection.demoEntities.Order_Products.Where(x => x.ID_Order == _currentOrder.ID && x.ID_Product == productId).First();
                orderProduct.ID_Product = ArticleCB.SelectedIndex + 1;

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
