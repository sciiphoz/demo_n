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
    /// Логика взаимодействия для EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        private Product _currentProduct;
        public EditProductWindow()
        {
            InitializeComponent();
        }
        public EditProductWindow(Product product)
        {
            InitializeComponent();

            MeasurementCB.ItemsSource = DataBaseConnection.demoEntities.Measurement.OrderBy(x => x.ID).Select(x => x.Name).ToList();
            CategoryCB.ItemsSource = DataBaseConnection.demoEntities.Category.OrderBy(x => x.ID).Select(x => x.Name).ToList();
            ManufacturerCB.ItemsSource = DataBaseConnection.demoEntities.Manufacturer.OrderBy(x => x.ID).Select(x => x.Name).ToList();
            SupplierCB.ItemsSource = DataBaseConnection.demoEntities.Supplier.OrderBy(x => x.ID).Select(x => x.Name).ToList();

            _currentProduct = product;

            ImageSourceConverter converter = new ImageSourceConverter();
            ImageSource imageSource = (ImageSource)converter.ConvertFromString(_currentProduct.Image == null ? _currentProduct.Image : "/Media/Products/picture.png");
            ProductImage.Source = imageSource;

            IDTB.Text = _currentProduct.ID.ToString();
            DescTB.Text = _currentProduct.Desc;
            DiscountTB.Text = _currentProduct.Discount.ToString();
            NameTB.Text = _currentProduct.Name;
            PriceTB.Text = _currentProduct.Price.ToString();
            QuantityTB.Text = _currentProduct.Quantity.ToString();

            MeasurementCB.SelectedIndex = _currentProduct.ID_Measurement - 1;
            CategoryCB.SelectedIndex = _currentProduct.ID_Category - 1;
            ManufacturerCB.SelectedIndex = _currentProduct.ID_Manufacturer - 1;
            SupplierCB.SelectedIndex = _currentProduct.ID_Supplier - 1;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _currentProduct.Desc = DescTB.Text;
                _currentProduct.Discount = Convert.ToDecimal(DiscountTB.Text);
                _currentProduct.Name = NameTB.Text;
                _currentProduct.Price = Convert.ToDecimal(PriceTB.Text);
                _currentProduct.Quantity = Convert.ToInt32(QuantityTB.Text);

                _currentProduct.ID_Measurement = MeasurementCB.SelectedIndex + 1;
                _currentProduct.ID_Category = CategoryCB.SelectedIndex + 1;
                _currentProduct.ID_Manufacturer = ManufacturerCB.SelectedIndex + 1;
                _currentProduct.ID_Supplier = SupplierCB.SelectedIndex + 1;

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
