using Demo3.DBContext;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        private string _newImage;
        private Product _currentProduct;
        public EditProductWindow()
        {
            InitializeComponent();

        }
        public EditProductWindow(Product product)
        {
            InitializeComponent();

            _currentProduct = product;

            ProductImage.Source = new BitmapImage(new Uri(_currentProduct.FullImagePath, UriKind.RelativeOrAbsolute));

            CategoryCB.ItemsSource = DataBaseConnection.demoEntities.Category.OrderBy(x => x.ID).Select(x => x.Name).ToList();
            ManufacturerCB.ItemsSource = DataBaseConnection.demoEntities.Manufacturer.OrderBy(x => x.ID).Select(x => x.Name).ToList();
            MeasurementCB.ItemsSource = DataBaseConnection.demoEntities.Measurement.OrderBy(x => x.ID).Select(x => x.Name).ToList();
            SupplierCB.ItemsSource = DataBaseConnection.demoEntities.Supplier.OrderBy(x => x.ID).Select(x => x.Name).ToList();

            DescTB.Text = _currentProduct.Desc;
            DiscountTB.Text = _currentProduct.Discount.ToString();
            IDTB.Text = _currentProduct.ID.ToString();
            NameTB.Text = _currentProduct.Name;
            PriceTB.Text = _currentProduct.Price.ToString();
            QuantityTB.Text = _currentProduct.Quantity.ToString();

            CategoryCB.SelectedIndex = _currentProduct.ID_Category - 1;
            ManufacturerCB.SelectedIndex = _currentProduct.ID_Manufacturer - 1;
            MeasurementCB.SelectedIndex = _currentProduct.ID_Measurement - 1;
            SupplierCB.SelectedIndex = _currentProduct.ID_Supplier - 1;
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "PNG|*.png|JPG|*.jpg";

            if (dialog.ShowDialog() == true)
            {
                _newImage = dialog.FileName;
                ProductImage.Source = new BitmapImage(new Uri(_newImage));
            }
        }

        private string SaveImage(string file)
        {
            string name = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file);

            string projectBase = AppDomain.CurrentDomain.BaseDirectory;
            string projectDir = new System.IO.DirectoryInfo(projectBase).Parent.Parent.FullName;
            string projectFolder = System.IO.Path.Combine(projectDir, "Media", "Products");
            File.Copy(file, System.IO.Path.Combine(projectFolder, name), true);

            string debugFolder = System.IO.Path.Combine(projectBase, "Media", "Products");
            System.IO.Directory.CreateDirectory(debugFolder);
            File.Copy(file, System.IO.Path.Combine(debugFolder, name), true);

            return "Media/Products/" + name;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _currentProduct.Desc = DescTB.Text;
                _currentProduct.Discount = Convert.ToDecimal(DiscountTB.Text);
                _currentProduct.Name = NameTB.Text;
                _currentProduct.Price = Convert.ToDecimal(PriceTB.Text);
                _currentProduct.Quantity = Convert.ToInt32(QuantityTB.Text);
                _currentProduct.Image = _newImage != null ? SaveImage(_newImage) : null;

                _currentProduct.ID_Category = CategoryCB.SelectedIndex + 1;
                _currentProduct.ID_Manufacturer = ManufacturerCB.SelectedIndex + 1;
                _currentProduct.ID_Measurement = MeasurementCB.SelectedIndex + 1;
                _currentProduct.ID_Supplier = SupplierCB.SelectedIndex + 1;

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
