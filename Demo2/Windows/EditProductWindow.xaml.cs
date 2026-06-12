using Demo2.DataBaseContext;
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

namespace Demo2.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        private Product _currentProduct;
        private string _newImage = null;
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
            LoadImageFromDB();

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

                if (_newImage != null)
                {
                    if (!string.IsNullOrEmpty(_currentProduct.Image) && _currentProduct.Image != "/Media/Products/picture.png")
                    {
                        ProductImage.Source = null;

                        string folder = AppDomain.CurrentDomain.BaseDirectory + "Media\\Products\\";
                        string oldFile = System.IO.Path.GetFileName(_currentProduct.Image);
                        string oldPath = System.IO.Path.Combine(folder, oldFile);

                        try
                        {
                            if (System.IO.File.Exists(oldPath))
                                System.IO.File.Delete(oldPath);
                        }
                        catch { }
                    }
                    _currentProduct.Image = SaveImage(_newImage);
                }

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

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "JPG|*.jpg|PNG|*.png";

            if (dialog.ShowDialog() == true)
            {
                _newImage = dialog.FileName;

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(dialog.FileName);
                bitmap.EndInit();
                ProductImage.Source = bitmap;
            }
        }

        private string SaveImage(string file)
        {
            string name = Guid.NewGuid().ToString() + ".jpg";
            string folder = AppDomain.CurrentDomain.BaseDirectory + "Media\\Products\\";

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fullPath = System.IO.Path.Combine(folder, name);

            var image = new BitmapImage(new Uri(file));
            double ratio = Math.Min(300.0 / image.Width, 200.0 / image.Height);

            if (ratio < 1)
            {
                var resized = new TransformedBitmap(image, new ScaleTransform(ratio, ratio));
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(resized));
                using (var stream = new FileStream(fullPath, FileMode.Create))
                    encoder.Save(stream);
            }
            else
                File.Copy(file, fullPath, true);

            return "/Media/Products/" + name;
        }
        private void LoadImageFromDB()
        {
            string imagePath = _currentProduct.Image;

            if (string.IsNullOrEmpty(imagePath) || imagePath == "/Media/Products/picture.png")
            {
                ProductImage.Source = new BitmapImage(new Uri("/Media/Products/picture.png", UriKind.Relative));
                return;
            }

            string folder = AppDomain.CurrentDomain.BaseDirectory + "Media\\Products\\";
            string fileName = System.IO.Path.GetFileName(imagePath);
            string fullPath = System.IO.Path.Combine(folder, fileName);

            if (File.Exists(fullPath))
                ProductImage.Source = new BitmapImage(new Uri(fullPath));
            else
                ProductImage.Source = new BitmapImage(new Uri("/Media/Products/picture.png", UriKind.Relative));
        }
    }
}
